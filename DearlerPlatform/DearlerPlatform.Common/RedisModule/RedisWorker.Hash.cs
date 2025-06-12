using System.Reflection;
using DearlerPlatform.Common.RedisModule.DTO;
using StackExchange.Redis;

namespace DearlerPlatform.Common.RedisModule;

public partial class RedisWorker
{
    public async Task SetHashMemoryAsync(string key, Dictionary<string, string> values)
    {
        var hashEntrys = values
        .Select(value => new HashEntry(value.Key, value.Value))
        .ToArray();
        await _redis.Db.HashSetAsync(key.ToString(), hashEntrys);
    }

    public async Task SetHashMemoryAsync(string key, HashEntry[] entries)
    {
        await _redis.Db.HashSetAsync(key, entries);
    }

    public async Task SetHashMemoryAsync<T>(string key, T data)
    {
        var props = typeof(T).GetProperties();
        var entries = new List<HashEntry>();
        foreach (var prop in props)
        {
            if (!prop.CanRead) continue;
            var value = prop.GetValue(data);
            if (value != null)
            {
                entries.Add(new HashEntry(prop.Name, value.ToString()));
            }
        }
        await _redis.Db.HashSetAsync(key, entries.ToArray());
    }

    public async Task<Dictionary<string, string>> GetHashValueAsync(string key, string valuekey)
    {
        var value = await _redis.Db.HashGetAsync(key, valuekey);
        return new Dictionary<string, string>() { { valuekey, value.ToString() } };
    }

    public async Task SetHashListMemoryAsync<T>(string key, IEnumerable<T> entities, Func<T, IEnumerable<string>> valueKeys)
    {
        Type type = typeof(T);
        PropertyInfo[] props = type.GetProperties();
        foreach (var entity in entities)
        {
            List<HashEntry> hashEntries = new();
            foreach (var prop in props)
            {
                string name = prop.Name;
                var value = prop.GetValue(entity);
                if (value == null) continue;
                hashEntries.Add(new HashEntry(name, value.ToString()));
            }
            await SetHashMemoryAsync($"{key}:{string.Join(":", valueKeys(entity))}", hashEntries.ToArray());
        }
    }

    public async Task<List<T>> GetHashMemoryAsync<T>(IEnumerable<string> keys) where T : new()
    {
        List<T> resList = new();
        var props = typeof(T).GetProperties();
        HashSet<RedisKey> relatedKeysList = new();
        foreach (var key in keys)
        {
            List<RedisKey> relatedKeys = await GetallKeysAsync(key);
            relatedKeysList.UnionWith(relatedKeys);
        }
        foreach (var relatedKey in relatedKeysList)
        {

            var res = await _redis.Db.HashGetAllAsync(relatedKey);
            if (res.Length == 0) continue;
            T t = new();
            foreach (var item in res)
            {
                foreach (var prop in props)
                {
                    if (prop.Name == item.Name)
                    {
                        if (!prop.CanWrite) continue;
                        prop.SetValue(t, Convert.ChangeType(item.Value.ToString(), prop.PropertyType));
                    }
                }
            }
            resList.Add(t);
        }
        return resList;
    }

        public async Task<List<T>> GetHashMemoryAsync<T>(string key) where T : new()
    {
        List<T> resList = new();
        var props = typeof(T).GetProperties();
        HashSet<RedisKey> relatedKeysList = new();
        List<RedisKey> relatedKeys = await GetallKeysAsync(key);
        relatedKeysList.UnionWith(relatedKeys);
        foreach (var relatedKey in relatedKeysList)
        {

            var res = await _redis.Db.HashGetAllAsync(relatedKey);
            if (res.Length == 0) continue;
            T t = new();
            foreach (var item in res)
            {
                foreach (var prop in props)
                {
                    if (prop.Name == item.Name)
                    {
                        prop.SetValue(t, Convert.ChangeType(item.Value.ToString(), prop.PropertyType));
                    }
                }
            }
            resList.Add(t);
        }
        return resList;
    }
}
