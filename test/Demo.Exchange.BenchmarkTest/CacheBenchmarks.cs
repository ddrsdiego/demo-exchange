namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Demo.Exchange.Infra.Cache;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.DependencyInjection;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class CacheBenchmarks
    {
        private static ICacheService cacheService;
        private static IDistributedCache distributedCache;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var serviceProvider = ContainerConfiguration.Configure();

            cacheService = serviceProvider.GetService<ICacheService>();
            distributedCache = serviceProvider.GetService<IDistributedCache>();
        }

        [Benchmark]
        public async Task CacheService()
        {
            var result = await cacheService.GetCacheValueAsString("STACKEXCHANGE-STRING-VAREJO");
            Debug.WriteLine(result);
        }

        [Benchmark]
        public async Task DistributedCache()
        {
            var result = await distributedCache.GetStringAsync("DISTRIBUTEDCACHE-STRING-VAREJO");
            Debug.WriteLine(result);
        }
    }
}