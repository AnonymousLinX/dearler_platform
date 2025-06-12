using System.Threading.Tasks;
using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Core.Core;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ShoppingCartApp.DTO;
using Microsoft.EntityFrameworkCore;

namespace DearlerPlatform.Service.ShoppingCartApp;

public partial class ShoppingCartService : IShoppingCartService
{
    private readonly IRepository<ShoppingCart> _cartRepo;
    private readonly IMapper _mapper;
    private readonly LocalEventBus<List<ShoppingCartDTO>> _localEventBusShoppingCartDTO;
    private readonly IProductService _productService;
    private readonly IRedisWorker _redisWorker;
    public ShoppingCartService(IRepository<ShoppingCart> cartRepo,
    IMapper mapper,
    LocalEventBus<List<ShoppingCartDTO>> localEventBusShoppingCartDTO,
    IProductService productService,
    IRedisWorker redisWorker)
    {
        _cartRepo = cartRepo;
        _mapper = mapper;
        _localEventBusShoppingCartDTO = localEventBusShoppingCartDTO;
        _productService = productService;
        _redisWorker = redisWorker;
    }

    public async Task<ShoppingCart> SetShoppingCart(ShoppingCartInputDTO input)
    {
        ShoppingCart shoppingCartres;
        // var shoppingCartRepeat = await _cartRepo.GetAsync(m => m.ProductNo == input.ProductNo);
        var shoppingCartRepeats = await _redisWorker.GetHashMemoryAsync<ShoppingCart>(new List<string> { $"ShoppingCart:*:{input.CustomerNo}" });
        var shoppingCartRepeat = shoppingCartRepeats.FirstOrDefault(m => m.ProductNo == input.ProductNo);
        if (shoppingCartRepeat != null)
        {
            shoppingCartRepeat.ProductNum++;
            shoppingCartres = await _cartRepo.UpdateAsync(shoppingCartRepeat);
        }
        else
        {
            var shoppingCart = _mapper.Map<ShoppingCartInputDTO, ShoppingCart>(input);
            shoppingCart.CartGuid = Guid.NewGuid().ToString();
            shoppingCart.CartSelected = true;
            // shoppingCartres = await _cartRepo.InsertAsync(shoppingCart);
            await _redisWorker.SetHashMemoryAsync(
                $"ShoppingCart:{shoppingCart.CartGuid}:{shoppingCart.CustomerNo}",
                RedisWorker.ObjectToStringDictionary<ShoppingCart>(shoppingCart)
                );
            return shoppingCart;
        }

        return shoppingCartres;
    }

    public async Task<List<ShoppingCartDTO>> GetShoppingCartDTOs(string customerNo)
    {
        var carts = await _redisWorker.GetHashMemoryAsync<ShoppingCart>(new List<string> { $"ShoppingCart:*:{customerNo}" });
        var dtos = _mapper.Map<List<ShoppingCart>, List<ShoppingCartDTO>>(carts);
        await _localEventBusShoppingCartDTO.PublishAsync(dtos);
        return dtos;
    }

    public async Task<bool> UpdateCartSelected(List<ShoppingCartSelectedEditDTO> edits, string customerNo)
    {
        /// <summary>
        /// 交互数据库
        /// </summary>
        // using (var context = new DealerPlatformContext())
        // {
        //     foreach (var editProduct in edits)
        //     {
        //         var cart = context.ShoppingCarts.FirstOrDefault(m => m.CartGuid == editProduct.CartGuids);
        //         if (cart == null) continue;
        //         cart.CartSelected = editProduct.CartSelected;
        //         cart.ProductNum = editProduct.ProductNum;
        //         context.Entry(cart).Property(p => p.CartSelected).IsModified = true;
        //         context.Entry(cart).Property(p => p.ProductNum).IsModified = true;
        //     }
        //     return context.SaveChanges() > 0;
        // }
        /// <summary>
        /// 交互redis
        /// </summary>

        foreach (var editProduct in edits)
        {
            try
            {
                var cart = await _redisWorker.GetHashMemoryAsync<ShoppingCart>(new List<string> { $"ShoppingCart:{editProduct.CartGuid}:*" });
                if (cart.Count == 0) continue;
                cart[0].CartSelected = editProduct.CartSelected;
                cart[0].ProductNum = editProduct.ProductNum;
                await _redisWorker.SetHashMemoryAsync<ShoppingCart>($"ShoppingCart:{editProduct.CartGuid}:{customerNo}", cart[0]);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"更新失败:{ex.Message}");
            }
        }
            return true;
    }
    public async Task<int> GetShoppingCartNum(string customerNo)
    {
        var carts = await _redisWorker.GetHashMemoryAsync<ShoppingCart>(new List<string> { $"ShoppingCart:*:{customerNo}" });
        // var carts = await _cartRepo.GetListAsync(m => m.CustomerNo == customerNo && m.CartSelected);
        var cartSelectedNum = 0;
        carts.ForEach(c =>
        {
            cartSelectedNum += c.ProductNum;
        });
        return cartSelectedNum;
    }
}
