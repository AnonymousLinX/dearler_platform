using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DearlerPlatform.Common.RedisModule;

public class RedisCore
{
    public readonly IDatabase Db;
    public readonly IServer Server;
    public RedisCore(string connectionStrings)
    {
        var serverAddres = connectionStrings.Split(",")[0].Trim();
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionStrings);
        IDatabase database = redis.GetDatabase();
        Db = database;
        var endpoint = redis.GetEndPoints().FirstOrDefault();
        IServer server = redis.GetServer(endpoint);
        Server = server;
    }
}
