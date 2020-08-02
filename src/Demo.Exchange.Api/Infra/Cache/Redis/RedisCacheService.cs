namespace Demo.Exchange.Infra.Cache.Redis
{
    using OpenTracing;
    using StackExchange.Redis;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ITracer _tracer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ITracer tracer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _tracer = tracer;
        }

        public async Task<T> GetCacheValue<T>(string key)
        {
            var operation = $"RedisCacheService::GetCacheValue::{key}";
            using var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true);
            {
                var value = await GetCacheValueAsString(key);
                return JsonSerializer.Deserialize<T>(value);
            }
        }

        public async Task<string> GetCacheValueAsString(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var operation = $"RedisCacheService::GetCacheValueAsString::{key}";

            using var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true);
            return await db.StringGetAsync(key);
        }

        public async Task SetCacheValue(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
        }
    }
}