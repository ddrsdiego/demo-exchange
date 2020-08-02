namespace Demo.Exchange.Infra.Cache
{
    using System.Threading.Tasks;

    public interface ICacheService
    {
        Task<T> GetCacheValue<T>(string key);

        Task<string> GetCacheValueAsString(string key);

        Task SetCacheValue(string key, string value);
    }
}