namespace DearlerPlatform.Common.RedisModule.DTO;

public class RedisShoppingCartDTO : RedisDTO
{
    public required string RedisName { get; set; }
    public required string ProductNo { get; set; }
    public required string customerNo { get; set; }
    public required Dictionary<string, string> RedisValue { get; set; }
    public override string ToString()
    {
        return $"{RedisName}:{ProductNo}:{customerNo}";
    }
}
