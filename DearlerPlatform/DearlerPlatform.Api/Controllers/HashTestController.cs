using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Common.RedisModule.DTO;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace DearlerPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HashTestController : ControllerBase
{
    private readonly IRedisWorker _redisWorker;
    public HashTestController(IRedisWorker redisWorker)
    {
        _redisWorker = redisWorker;
    }
    [HttpGet]
    public async Task<string> GetString(string userName)
    {
        await _redisWorker.SetStringAsync("UserName", userName, TimeSpan.FromSeconds(20));
        return await _redisWorker.GetStringAsync("UserName");
    }
    [HttpPost]
    public async Task SetHash(RedisShoppingCartDTO redisShoppingcart)
    {
        await _redisWorker.SetHashMemoryAsync(redisShoppingcart.ToString(), redisShoppingcart.RedisValue);
    }
    [HttpGet("hash")]
    public async Task<Dictionary<string, string>> GetHash(string key, string valuekey)
    {
        return await _redisWorker.GetHashValueAsync(key, valuekey);
    }
    [HttpPost("SetHash")]
    public async Task SetHashList(List<UserInfo> userInfos)
    {
        await _redisWorker.SetHashListMemoryAsync<UserInfo>("ShoppingCart", userInfos, m =>
        {
            return new List<string>() { m.Id.ToString(), m.UserName };
        });
    }
    [HttpGet("GetHash")]
    public async Task<List<UserInfo>> GetHashEntriesAsync(string keys)
    {
        return await _redisWorker.GetHashMemoryAsync<UserInfo>(keys.Split("&"));
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
    } 
}
