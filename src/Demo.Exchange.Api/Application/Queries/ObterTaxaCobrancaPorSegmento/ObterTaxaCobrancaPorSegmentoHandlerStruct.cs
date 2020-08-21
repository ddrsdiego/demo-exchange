namespace Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento
{
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Cache;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class ObterTaxaCobrancaPorSegmentoHandlerStruct : Handler, IRequestHandler<ObterTaxaCobrancaPorSegmentoQueryStruct, ObterTaxaCobrancaPorSegmentoResponseStruct>
    {
        private readonly ICacheService _cacheService;
        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public ObterTaxaCobrancaPorSegmentoHandlerStruct(IMediator mediator, ILoggerFactory logger, ITaxaCobrancaRepository taxaCobrancaRepository, ICacheService cacheService)
            : base(mediator, logger.CreateLogger<ObterTaxaCobrancaPorSegmentoHandlerStruct>())
        {
            _cacheService = cacheService;
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<ObterTaxaCobrancaPorSegmentoResponseStruct> Handle(ObterTaxaCobrancaPorSegmentoQueryStruct request, CancellationToken cancellationToken)
        {
            var response = new ObterTaxaCobrancaPorSegmentoResponseStruct();

            var tipoSegmento = TipoSegmento.ObterPorIdIf(request.TipoSegmento);
            if (tipoSegmento is null)
            {
                response.AddError(Errors.General.NotFound(nameof(TipoSegmento), request.TipoSegmento));
                return response;
            }

            response.PayLoad = await _cacheService.GetCacheValueAsString($"STACKEXCHANGE-STRING-{tipoSegmento.Id}");

            return response;
        }
    }
}