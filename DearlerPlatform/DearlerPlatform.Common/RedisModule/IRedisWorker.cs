using DearlerPlatform.Common.RedisModule.DTO;
using StackExchange.Redis;

namespace DearlerPlatform.Common.RedisModule;

public interface IRedisWorker
{
    public Task<string> GetStringAsync(string key);
    public Task SetStringAsync(string key, string value, TimeSpan time);
    public Task SetHashMemoryAsync(string key, Dictionary<string, string> values);
    public Task SetHashMemoryAsync(string key, HashEntry[] entries);
    public Task SetHashMemoryAsync<T>(string key, T data);
    public Task<Dictionary<string, string>> GetHashValueAsync(string key, string valuekey);
    public Task SetHashListMemoryAsync<T>(string key, IEnumerable<T> entities, Func<T, IEnumerable<string>> valueKeys);
    public Task<List<T>> GetHashMemoryAsync<T>(IEnumerable<string> keys) where T : new();
}
