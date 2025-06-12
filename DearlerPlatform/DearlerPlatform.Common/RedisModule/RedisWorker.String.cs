using StackExchange.Redis;

namespace DearlerPlatform.Common.RedisModule;

public partial class RedisWorker
{
    public async Task SetStringAsync(string key, string value, TimeSpan time)
    {
        await _redis.Db.StringSetAsync(key, value, time);
    }

    public async Task<string> GetStringAsync(string key)
    {
        return await _redis.Db.StringGetAsync(key);
    }
}
