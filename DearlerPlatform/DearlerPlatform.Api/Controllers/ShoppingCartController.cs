using System.Threading.Tasks;
using DearlerPlatform.Api.Filters;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShoppingCartApp;
using DearlerPlatform.Service.ShoppingCartApp.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Api.Controllers;


[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ShoppingCartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [HttpPost]
    [CustomerAuthorizationFilter]
    public async Task<ShoppingCart> SetShoppingCart(ShoppingCartInputDTO input)
    {
        var customerNo = HttpContext.Items["CustomerNo"]?.ToString();
        input.CustomerNo = customerNo;
        return await _shoppingCartService.SetShoppingCart(input);
    }


    [HttpGet]
    [CustomerAuthorizationFilter]
    public async Task<dynamic> GetShoppingCartDTOsAsync()
    {
        var customerNo = HttpContext.Items["CustomerNo"]?.ToString();
        var carts = await _shoppingCartService.GetShoppingCartDTOs(customerNo);
        var productDtos = carts.Select(m => m.ProductDto);
        var type = productDtos.Select(m => new { m.TypeNo, m.TypeName }).Distinct();
        return new { carts, type };
    }

    [HttpPost("CartSelected")]
    [CustomerAuthorizationFilter]
    public async Task<bool> UpdateCartSelected(List<ShoppingCartSelectedEditDTO> edit)
    {
        var customerNo = HttpContext.Items["CustomerNo"]?.ToString();
        return await _shoppingCartService.UpdateCartSelected(edit, customerNo);
    }

    [HttpGet("num")]
    [CustomerAuthorizationFilter]
    public async Task<int> GetShoppingCartNum()
    {
        var customerNo = HttpContext.Items["CustomerNo"]?.ToString();
        return await _shoppingCartService.GetShoppingCartNum(customerNo);
    }
}
