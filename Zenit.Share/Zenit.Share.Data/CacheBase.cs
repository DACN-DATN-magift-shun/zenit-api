using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data
{
    public abstract class CacheBase : ICache
    {
        public abstract Task<T> GetAsync<T>(string key);
        public abstract Task AddAsync<T>(string key, T value, DateTimeOffset expiration);
        public abstract Task RemoveAsync(string key);
    }
}