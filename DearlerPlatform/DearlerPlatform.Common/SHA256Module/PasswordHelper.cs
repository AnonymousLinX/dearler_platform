using System.Security.Cryptography;
using System.Text;

namespace DearlerPlatform.Common.SHA256Module;

public static class PasswordHelper
{
    public static string ToSHA256(this string password)
    {
        string salt = "DearlerPlatformSalt@2025";
        using (var sha256 = SHA256.Create())
        {
            var passwordSaltBytes = Encoding.UTF8.GetBytes(password + salt);
            var hashBytes = sha256.ComputeHash(passwordSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
    // 项目数据库未设计盐值暂不启用
    // public static string ToSHA256(string password, string salt)
    // {
    //     using (var sha256 = SHA256.Create())
    //     {
    //         var passwordSaltBytes = Encoding.UTF8.GetBytes(password + salt);
    //         var hashBytes = sha256.ComputeHash(passwordSaltBytes);
    //         return Convert.ToBase64String(hashBytes);
    //     }
    // }
    // 生成随机盐值
    // public static string CreateSalt(int size = 16)
    // {
    //     var saltBytes = new byte[size];
    //     using (var randomNum = RandomNumberGenerator.Create())
    //     {
    //         randomNum.GetBytes(saltBytes);
    //     }
    //     return Convert.ToBase64String(saltBytes);
    // }
}
