namespace Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento
{
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Cache;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class ObterTaxaCobrancaPorSegmentoHandler : Handler, IRequestHandler<ObterTaxaCobrancaPorSegmentoQuery, ObterTaxaCobrancaPorSegmentoResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public ObterTaxaCobrancaPorSegmentoHandler(IMediator mediator, ILoggerFactory logger, ITaxaCobrancaRepository taxaCobrancaRepository, ICacheService cacheService)
            : base(mediator, logger.CreateLogger<ObterTaxaCobrancaPorSegmentoHandler>())
        {
            _cacheService = cacheService;
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<ObterTaxaCobrancaPorSegmentoResponse> Handle(ObterTaxaCobrancaPorSegmentoQuery request, CancellationToken cancellationToken)
        {
            var response = (ObterTaxaCobrancaPorSegmentoResponse)request.Response;

            var tipoSegmento = TipoSegmento.ObterPorIdIf(request.TipoSegmento);
            if (tipoSegmento is null)
            {
                response.AddError(Errors.General.NotFound("TipoSegmento", request.TipoSegmento));
                return response;
            }

            var tipoSegmentoResponse = await _cacheService.GetCacheValueAsString($"STACKEXCHANGE-STRING-{tipoSegmento.Id}");

            response.SetPayLoad(tipoSegmentoResponse);

            return response;
        }

        private async Task<TaxaResponse> ObterTaxaCobrancaPorSegmento(ObterTaxaCobrancaPorSegmentoQuery request, ObterTaxaCobrancaPorSegmentoResponse response, TipoSegmento tipoSegmento)
        {
            var taxaCobranca = await _taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(tipoSegmento.Id);
            if (string.IsNullOrEmpty(taxaCobranca.TaxaCobrancaId))
            {
                response.AddError(Errors.General.NotFound(nameof(TaxaCobranca), request.TipoSegmento));
                Logger.LogWarning($"{response.ErrorResponse}");

                return default;
            }

            return taxaCobranca.ConverterEntidadeParaResponse();
        }
    }
}