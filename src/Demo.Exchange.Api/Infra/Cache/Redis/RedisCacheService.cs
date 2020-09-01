namespace Demo.Exchange.Infra.Cache.Redis
{
    using StackExchange.Redis;
    using System.Text.Json;
    using System.Threading.Tasks;

    public sealed class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer) => _connectionMultiplexer = connectionMultiplexer;

        public async ValueTask<T> GetCacheValue<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(await GetCacheValueAsString(key));
        }

        public async ValueTask<byte[]> GetCacheValueAsByte(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async ValueTask<string> GetCacheValueAsString(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async ValueTask SetCacheValueAsByte(string key, byte[] value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async ValueTask SetCacheValueAsString(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
        }
    }
}