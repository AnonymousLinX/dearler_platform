using StackExchange.Redis;

namespace DearlerPlatform.Common.RedisModule;

public partial class RedisWorker : IRedisWorker
{
    private readonly RedisCore _redis;
    public RedisWorker(RedisCore redisCore)
    {
        _redis = redisCore;
    }
    public async Task<List<RedisKey>> GetallKeysAsync(string key)
    {
        var keys = new List<RedisKey>();
        Console.WriteLine(_redis.Server.IsConnected);
        var keyEnumerator = _redis.Server.KeysAsync(pattern: key).GetAsyncEnumerator();
        while (await keyEnumerator.MoveNextAsync())
        {
            keys.Add(keyEnumerator.Current);
        }
        return keys;
    }

    public static Dictionary<string, string> ObjectToStringDictionary<T>(T obj)
    {
        return typeof(T).GetProperties()
        .Where(p => p.CanRead)
        .ToDictionary(
            p => p.Name,
            p => p.GetValue(obj)?.ToString() ?? ""
        );
    }

    public void RemoveKey(string key)
    { 
        _redis.Db.KeyDelete(key);
    }
}
