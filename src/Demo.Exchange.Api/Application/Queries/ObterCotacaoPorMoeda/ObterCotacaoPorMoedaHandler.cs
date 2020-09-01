namespace Demo.Exchange.Application.Queries.ObterCotacaoPorMoeda
{
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Domain.Services.CadernoFomulas;
    using Demo.Exchange.Infra.Cache.Memcached;
    using Demo.Exchange.Infra.Connectors;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ObterCotacaoPorMoedaHandler : Handler, IRequestHandler<ObterCotacaoPorMoedaQuery, Response>
    {
        private TipoSegmento TipoSegmento;
        private TaxaResponse TaxaResponse;
        private ParametroOutput ValorConversao;
        private KeyValuePair<string, decimal> Cotacao;

        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICadernoFormulasService _cadernoFormulasService;
        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;
        private readonly IExchangeRatesApiConnector _exchangeRatesApiConnector;

        public ObterCotacaoPorMoedaHandler(IMediator mediator,
                                           ILoggerFactory logger,
                                           IExchangeRatesApiConnector exchangeRatesApiConnector,
                                           ICadernoFormulasService cadernoFormulasService,
                                           ITaxaCobrancaRepository taxaCobrancaRepository,
                                           ICacheProvider cacheProvider,
                                           ICacheRepository cacheRepository)
            : base(mediator, logger.CreateLogger<ObterCotacaoPorMoedaHandler>())
        {
            _cacheProvider = cacheProvider;
            _cacheRepository = cacheRepository;
            _cadernoFormulasService = cadernoFormulasService;
            _taxaCobrancaRepository = taxaCobrancaRepository;
            _exchangeRatesApiConnector = exchangeRatesApiConnector;
        }

        public async Task<Response> Handle(ObterCotacaoPorMoedaQuery request, CancellationToken cancellationToken)
        {
            Response response;

            response = ObterCotacaoPorMoedaQueryValidator.ValidarQuery(request);
            if (response.IsFailure)
                return response;

            response = ObterTipoSegmentoPorId(request);
            if (response.IsFailure)
                return response;

            response = await ObterTaxaCobrancaPorSegmento(request);
            if (response.IsFailure)
                return response;

            response = await OberUltimaCotacaoPorMoeda(request);
            if (response.IsFailure)
                return response;

            response = ExecutarCalculoCotacao(request);
            if (response.IsFailure)
                return response;

            response = CreateResponse(request);
            if (response.IsFailure)
                return response;

            return response;
        }

        private async Task<Response> OberUltimaCotacaoPorMoeda(ObterCotacaoPorMoedaQuery request)
        {
            var response = Response.Ok();

            try
            {
                Cotacao = await _exchangeRatesApiConnector.OberUltimaCotacaoPorMoeda(request.Moeda);
                if (Cotacao.Equals(default))
                {
                    response = Response.Fail(Errors.General.NotFound("ModeEstrangeira", request.Moeda));
                    Logger.LogWarning($"{response.ErrorResponse}");

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = Response.Fail(Errors.General.InternalProcessError("OberUltimaCotacaoPorMoeda", ex.Message));
                Logger.LogError(ex, $"{response.ErrorResponse}");

                return response;
            }

            return response;
        }

        private Response ObterTipoSegmentoPorId(ObterCotacaoPorMoedaQuery request)
        {
            var requestResponse = Response.Ok();

            TipoSegmento = TipoSegmento.ObterPorIdIf(request.Segmento);
            if (TipoSegmento is null)
            {
                requestResponse = Response.Fail(Errors.General.NotFound(nameof(TipoSegmento), request.Segmento));
                Logger.LogWarning($"{requestResponse.ErrorResponse}");
            }

            return requestResponse;
        }

        private async Task<Response> ObterTaxaCobrancaPorSegmento(ObterCotacaoPorMoedaQuery request)
        {
            var response = Response.Ok();

            try
            {
                TaxaResponse = await ObterTaxaEstrategiCache(async () =>
                {
                    var taxaCobranca = await _taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(TipoSegmento.Id);
                    if (string.IsNullOrEmpty(taxaCobranca.TaxaCobrancaId))
                        return default;

                    return taxaCobranca.ConverterEntidadeParaResponse();
                }, async () => await _cacheProvider.Get<TaxaResponse>(TipoSegmento.Id));

                if (string.IsNullOrEmpty(TaxaResponse.Id))
                {
                    response = Response.Fail(Errors.General.NotFound(nameof(TaxaCobranca), request.Segmento));
                    Logger.LogWarning($"{response.ErrorResponse}");

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = Response.Fail(Errors.General.InternalProcessError("ObterTaxaCobrancaPorSegmento", ex.Message));
                Logger.LogError(ex, $"{response.ErrorResponse}");

                return response;
            }

            return response;
        }

        private async Task<TaxaResponse> ObterTaxaEstrategiCache(Func<Task<TaxaResponse>> fromRepo, Func<Task<TaxaResponse>> fromCache)
        {
            TaxaResponse taxaResponse = await fromCache();
            if (!string.IsNullOrEmpty(taxaResponse.Id))
                return taxaResponse;

            taxaResponse = await fromRepo();
            if (!string.IsNullOrEmpty(taxaResponse.Id))
                await _cacheRepository.Set(TipoSegmento.Id, taxaResponse);

            return taxaResponse;
        }

        private Response ExecutarCalculoCotacao(ObterCotacaoPorMoedaQuery request)
        {
            var response = Response.Ok();

            var formula = new ConversaoMoedaFormula(new ConversaoMoedaParametroInput(request.Quantidade, Cotacao.Value, TaxaResponse.ValorTaxa));

            var resultadoValorContacao = _cadernoFormulasService.Compute(formula);
            if (resultadoValorContacao.IsFailure)
            {
                var invalidQueryParameters = Errors.General
                                                .InvalidQueryParameters()
                                                .AddErroDetail(Errors.General.InvalidArgument("ParametroCalculoModeInvalidos", string.Join("|", resultadoValorContacao.Messages)));
                response = Response.Fail(invalidQueryParameters);
            }

            ValorConversao = resultadoValorContacao.Value;

            return response;
        }

        private Response CreateResponse(ObterCotacaoPorMoedaQuery request)
        {
            const int VALOR_UNITARIO_MOEDA_LOCAL = 1;

            var cotacaoPorMoedaResponse = new CotacaoPorMoedaResponse(new MoedaResponse(Cotacao.Key, VALOR_UNITARIO_MOEDA_LOCAL),
                                                new MoedaResponse(request.Moeda, Cotacao.Value),
                                                TaxaResponse.ValorTaxa,
                                                ValorConversao.Valor,
                                                request.Quantidade);

            return Response.Ok(ResponseContent.Create(cotacaoPorMoedaResponse));
        }
    }
}