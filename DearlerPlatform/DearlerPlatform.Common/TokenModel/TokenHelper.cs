using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DearlerPlatform.Common.TokenModel.Models;
using Microsoft.IdentityModel.Tokens;

namespace DearlerPlatform.Common.TokenModel;

public static class TokenHelper
{
    public static string CreatToken(JwtTokenModel jwtTokenModel)
    {
        var claims = new[]{
            new Claim("Id", jwtTokenModel.Id.ToString()),
            new Claim("CustomerNo", jwtTokenModel.CustomerNo),
            new Claim("CustomerName", jwtTokenModel.CustomerName)
        };
        // 生成密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenModel.Security));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtTokenModel.Issuer,
            audience: jwtTokenModel.Audience,
            expires: DateTime.Now.AddDays(jwtTokenModel.Expires),
            signingCredentials:creds,
            claims: claims
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return accessToken;
    }
}
