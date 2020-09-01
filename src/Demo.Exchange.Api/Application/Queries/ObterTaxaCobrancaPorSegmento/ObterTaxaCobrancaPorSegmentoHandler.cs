namespace Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento
{
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Cache;
    using Demo.Exchange.Infra.Cache.Memcached;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class ObterTaxaCobrancaPorSegmentoHandler : Handler, IRequestHandler<ObterTaxaCobrancaPorSegmentoQuery, Response>
    {
        private readonly ICacheService _cacheService;
        private readonly ICacheProvider _cacheProvider;
        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public ObterTaxaCobrancaPorSegmentoHandler(IMediator mediator,
                                                            ILoggerFactory logger,
                                                            ITaxaCobrancaRepository taxaCobrancaRepository,
                                                            ICacheProvider cacheProvider,
                                                            ICacheService cacheService)
            : base(mediator, logger.CreateLogger<ObterTaxaCobrancaPorSegmentoHandler>())
        {
            _cacheService = cacheService;
            _cacheProvider = cacheProvider;
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<Response> Handle(ObterTaxaCobrancaPorSegmentoQuery request, CancellationToken cancellationToken)
        {
            var tipoSegmento = TipoSegmento.ObterPorIdIf(request.TipoSegmento);
            if (tipoSegmento is null)
                return Response.Fail(Errors.General.NotFound(nameof(TipoSegmento), request.TipoSegmento));

            var taxaSegmento = await _cacheService.GetCacheValueAsByte($"BYTE-{tipoSegmento.Id}");
            if (taxaSegmento is null)
                return Response.Fail(Errors.General.NotFound(nameof(TipoSegmento), tipoSegmento.Id));

            return Response.Ok(ResponseContent.Create(taxaSegmento, typeof(TaxaResponse)));
        }
    }
}