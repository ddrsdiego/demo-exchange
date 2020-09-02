namespace Demo.Exchange.Application.Commands.RegistrarTaxaInCache
{
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Cache;
    using Demo.Exchange.Infra.Cache.Memcached;
    using MediatR;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RegistrarTaxaInCacheCommandHandler : Handler, IRequestHandler<RegistrarTaxaInCacheCommand, Response>
    {
        private TaxaCobranca TaxaCobranca;

        private readonly ICacheService _cacheService;
        private readonly ICacheRepository _cacheRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public RegistrarTaxaInCacheCommandHandler(IMediator mediator,
                                                  ILoggerFactory logger,
                                                  ITaxaCobrancaRepository taxaCobrancaRepository,
                                                  ICacheRepository cacheRepository,
                                                  ICacheService cacheService,
                                                  IDistributedCache distributedCache)
            : base(mediator, logger.CreateLogger<RegistrarTaxaInCacheCommandHandler>())
        {
            _cacheService = cacheService;
            _cacheRepository = cacheRepository;
            _distributedCache = distributedCache;
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<Response> Handle(RegistrarTaxaInCacheCommand request, CancellationToken cancellationToken)
        {
            var response = await ObterPorId(request);
            if (response.IsFailure)
                return response;

            var taxaResponse = TaxaCobranca.ConverterEntidadeParaResponse();

            var responseContent = ResponseContent.Create<TaxaResponse>(taxaResponse);

            await _cacheService.SetCacheValueAsByte($"BYTE-{taxaResponse.Id}", responseContent.Value);
            await _cacheService.SetCacheValueAsByte($"BYTE-{taxaResponse.TipoSegmento}", responseContent.Value);

            await _distributedCache.SetStringAsync($"DISTRIBUTEDCACHE-STRING-{taxaResponse.Id}", responseContent.ValueAsJsonString);
            await _distributedCache.SetStringAsync($"DISTRIBUTEDCACHE-STRING-{taxaResponse.TipoSegmento}", responseContent.ValueAsJsonString);

            await _cacheService.SetCacheValueAsString($"STACKEXCHANGE-STRING-{taxaResponse.Id}", responseContent.ValueAsJsonString);
            await _cacheService.SetCacheValueAsString($"STACKEXCHANGE-STRING-{taxaResponse.TipoSegmento}", responseContent.ValueAsJsonString);

            await _cacheRepository.Set(taxaResponse.Id, responseContent.Value);
            await _cacheRepository.Set(taxaResponse.TipoSegmento, responseContent.Value);

            return Response.Ok(responseContent);
        }

        private async Task<Response> ObterPorId(RegistrarTaxaInCacheCommand request)
        {
            try
            {
                TaxaCobranca = await _taxaCobrancaRepository.ObterPorId(request.Id);
                if (TaxaCobranca is null)
                    return Response.Fail(Errors.General.NotFound(nameof(TaxaCobranca), request.Id));
            }
            catch (Exception ex)
            {
                return Response.Fail(Errors.General.InternalProcessError("ObterTaxaPorId", ex.Message));
            }

            return Response.Ok();
        }
    }
}