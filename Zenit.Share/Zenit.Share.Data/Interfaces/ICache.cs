namespace Zenit.Share.Data.Interfaces
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);
        Task AddAsync<T>(string key, T value, DateTimeOffset expiration);
        Task RemoveAsync(string key);
    }
}