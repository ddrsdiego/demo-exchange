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

    public class ObterCotacaoPorMoedaHandler : Handler, IRequestHandler<ObterCotacaoPorMoedaQuery, ObterCotacaoPorMoedaResponse>
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

        public async Task<ObterCotacaoPorMoedaResponse> Handle(ObterCotacaoPorMoedaQuery request, CancellationToken cancellationToken)
        {
            var response = (ObterCotacaoPorMoedaResponse)request.Response;

            ObterCotacaoPorMoedaQueryValidator.ValidarQuery(request, response);
            if (response.IsFailure)
                return response;

            ObterTipoSegmentoPorId(request, response);
            if (response.IsFailure)
                return response;

            await ObterTaxaCobrancaPorSegmento(request, response);
            if (response.IsFailure)
                return response;

            await OberUltimaCotacaoPorMoeda(request, response);
            if (response.IsFailure)
                return response;

            ExecutarCalculoCotacao(request, response);
            if (response.IsFailure)
                return response;

            CreateResponse(request, response);

            return response;
        }

        private async Task OberUltimaCotacaoPorMoeda(ObterCotacaoPorMoedaQuery request, ObterCotacaoPorMoedaResponse response)
        {
            try
            {
                Cotacao = await _exchangeRatesApiConnector.OberUltimaCotacaoPorMoeda(request.Moeda);
                if (Cotacao.Equals(default))
                {
                    response.AddError(Errors.General.NotFound("ModeEstrangeira", request.Moeda));
                    Logger.LogWarning($"{response.ErrorResponse}");

                    return;
                }
            }
            catch (Exception ex)
            {
                response.AddError(Errors.General.InternalProcessError("OberUltimaCotacaoPorMoeda", ex.Message));
                Logger.LogError(ex, $"{response.ErrorResponse}");
            }
        }

        private void ObterTipoSegmentoPorId(ObterCotacaoPorMoedaQuery request, ObterCotacaoPorMoedaResponse response)
        {
            TipoSegmento = TipoSegmento.ObterPorIdIf(request.Segmento);
            if (TipoSegmento is null)
            {
                response.AddError(Errors.General.NotFound(nameof(TipoSegmento), request.Segmento));
                Logger.LogWarning($"{response.ErrorResponse}");

                return;
            }
        }

        private async Task ObterTaxaCobrancaPorSegmento(ObterCotacaoPorMoedaQuery request, ObterCotacaoPorMoedaResponse response)
        {
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
                    response.AddError(Errors.General.NotFound(nameof(TaxaCobranca), request.Segmento));
                    Logger.LogWarning($"{response.ErrorResponse}");
                    return;
                }
            }
            catch (Exception ex)
            {
                response.AddError(Errors.General.InternalProcessError("ObterTaxaCobrancaPorSegmento", ex.Message));
                Logger.LogError(ex, $"{response.ErrorResponse}");
            }
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

        private void ExecutarCalculoCotacao(ObterCotacaoPorMoedaQuery request, ObterCotacaoPorMoedaResponse response)
        {
            var formula = new ConversaoMoedaFormula(new ConversaoMoedaParametroInput(request.Quantidade, Cotacao.Value, TaxaResponse.ValorTaxa));

            var resultadoValorContacao = _cadernoFormulasService.Compute(formula);
            if (resultadoValorContacao.IsFailure)
            {
                var invalidQueryParameters = Errors.General
                                                .InvalidQueryParameters()
                                                .AddErroDetail(Errors.General.InvalidArgument("ParametroCalculoModeInvalidos", string.Join("|", resultadoValorContacao.Messages)));
                response.AddError(invalidQueryParameters);
            }

            ValorConversao = resultadoValorContacao.Value;
        }

        private void CreateResponse(ObterCotacaoPorMoedaQuery request, ObterCotacaoPorMoedaResponse response)
        {
            const int VALOR_UNITARIO_MOEDA_LOCAL = 1;

            response.SetPayLoad(new CotacaoPorMoedaResponse(
                new MoedaResponse(Cotacao.Key, VALOR_UNITARIO_MOEDA_LOCAL),
                new MoedaResponse(request.Moeda, Cotacao.Value),
                TaxaResponse.ValorTaxa,
                ValorConversao.Valor,
                request.Quantidade));
        }
    }
}