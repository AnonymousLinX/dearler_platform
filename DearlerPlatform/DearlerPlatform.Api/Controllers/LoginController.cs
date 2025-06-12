using DearlerPlatform.Common.TokenModel;
using DearlerPlatform.Common.TokenModel.Models;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Service.CustomerApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Api.Controllers;

[ApiController]
public class LoginController : BaseController
{
    public ICustomerService CustomerService { get; }
    public IConfiguration Configuration { get; set; }
    public LoginController(ICustomerService customerService, IConfiguration configuration)
    {
        CustomerService = customerService;
        Configuration = configuration;
    }
    [HttpPost]
    public async Task<string> Login(CustomerLoginDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CustomerNo) || string.IsNullOrWhiteSpace(dto.Password))
        {
            HttpContext.Response.StatusCode = 400;
            return "NonLoginInfo";
        }

        var isSuccess = await CustomerService.CheckPassworld(dto);
        if (isSuccess)
        {
            var customer = await CustomerService.GetCustomerAsync(dto.CustomerNo);
            return GetToken(customer.Id, customer.CustomerNo, customer.CustomerName);
        }
        else
        {
            HttpContext.Response.StatusCode = 401;
            return "NonUser";
        }
    }

    private string GetToken(int userId, string CustomerNo, string CustomerName)
    {
        var token = Configuration.GetSection("Jwt").Get<JwtTokenModel>();
        token.Id = userId;
        token.CustomerNo = CustomerNo;
        token.CustomerName = CustomerName;
        return TokenHelper.CreatToken(token);
    }
}
