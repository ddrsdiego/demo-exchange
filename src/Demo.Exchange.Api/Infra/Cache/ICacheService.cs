namespace Demo.Exchange.Infra.Cache
{
    using System.Threading.Tasks;

    public interface ICacheService
    {
        ValueTask<T> GetCacheValue<T>(string key);

        ValueTask<string> GetCacheValueAsString(string key);

        ValueTask<byte[]> GetCacheValueAsByte(string key);

        ValueTask SetCacheValueAsString(string key, string value);

        ValueTask SetCacheValueAsByte(string key, byte[] value);
    }
}