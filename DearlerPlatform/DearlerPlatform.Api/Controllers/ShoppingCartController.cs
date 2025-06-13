using System.Threading.Tasks;
using DearlerPlatform.Api.Filters;
using DearlerPlatform.Common.RedisModule;
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
    private readonly IRedisWorker _redisWorker;
    public ShoppingCartController(
        IShoppingCartService shoppingCartService,
        IRedisWorker redisWorker
        )
    {
        _shoppingCartService = shoppingCartService;
        _redisWorker = redisWorker;
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
    public async Task<bool> UpdateCartSelected(List<ShoppingCartSelectedEditDTO> edits)
    {
        var customerNo = HttpContext.Items["CustomerNo"]?.ToString();
        List<ShoppingCartSelectedEditDTO> newEdits = [];
        foreach (var cart in edits)
        {
            if (cart.ProductNum <= 0)
            {
                _redisWorker.RemoveKey($"ShoppingCart:{cart.CartGuid}:{customerNo}");
                continue;
            }
            newEdits.Add(cart);
        }
        return await _shoppingCartService.UpdateCartSelected(newEdits, customerNo);
    }

    [HttpGet("num")]
    [CustomerAuthorizationFilter]
    public async Task<int> GetShoppingCartNum()
    {
        var customerNo = HttpContext.Items["CustomerNo"]?.ToString();
        return await _shoppingCartService.GetShoppingCartNum(customerNo);
    }
}
