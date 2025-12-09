using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

using Zenit.Share.Data;

namespace Zenit.Management.Data
{
    public class ManagementRedisCache(IDistributedCache cache) : CacheBase
    {
        private IDistributedCache _cache = cache;

        public override async Task<T> GetAsync<T>(string key)
        {
            byte[]? cachedData = await _cache.GetAsync(key);

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<T>(cachedDataString);
            }
            return default;
        }

        public override async Task AddAsync<T>(string key, T value, DateTimeOffset expiration)
        {
            if (value == null)
            {
                return;
            }

            var serializedData = JsonSerializer.Serialize(value);
            var cachedDataString = Encoding.UTF8.GetBytes(serializedData);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiration
            };
            await _cache.SetAsync(key, cachedDataString, options);
        }

        public override async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}