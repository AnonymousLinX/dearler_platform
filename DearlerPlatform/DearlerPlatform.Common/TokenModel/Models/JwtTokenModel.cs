namespace DearlerPlatform.Common.TokenModel.Models;

public class JwtTokenModel
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int Expires { get; set; }
    public string Security { get; set; }
    public int Id { get; set; }
    public string CustomerNo { get; set; }
    public string CustomerName { get; set; }
}
