namespace Demo.Exchange.Infra.Cache.Redis
{
    using OpenTracing;
    using StackExchange.Redis;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class RedisCacheService : ICacheService
    {
        private readonly ITracer _tracer;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ITracer tracer)
        {
            _tracer = tracer;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async ValueTask<T> GetCacheValue<T>(string key)
        {
            var operation = $"RedisCacheService::GetCacheValue::{key}";
            using var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true);
            {
                return JsonSerializer.Deserialize<T>(await GetCacheValueAsString(key));
            }
        }

        public async ValueTask<byte[]> GetCacheValueAsByte(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var operation = $"RedisCacheService::GetCacheValueAsByte::{key}";

            using var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true);
            return await db.StringGetAsync(key);
        }

        public async ValueTask<string> GetCacheValueAsString(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var operation = $"RedisCacheService::GetCacheValueAsString::{key}";

            using var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true);
            return await db.StringGetAsync(key);
        }

        public async ValueTask SetCacheValueAsByte(string key, byte[] value)
        {
            var operation = $"RedisCacheService::SetCacheValueAsByte::{key}";

            using (var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true))
            {
                var db = _connectionMultiplexer.GetDatabase();
                await db.StringSetAsync(key, value);
            }
        }

        public async ValueTask SetCacheValueAsString(string key, string value)
        {
            var operation = $"RedisCacheService::SetCacheValueAsString::{key}";

            using (var scope = _tracer.BuildSpan(operation).StartActive(finishSpanOnDispose: true))
            {
                var db = _connectionMultiplexer.GetDatabase();
                await db.StringSetAsync(key, value);
            }
        }
    }
}