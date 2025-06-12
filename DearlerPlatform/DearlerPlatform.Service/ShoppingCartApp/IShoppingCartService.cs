using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShoppingCartApp.DTO;

namespace DearlerPlatform.Service.ShoppingCartApp;

public interface IShoppingCartService
{
    public Task<ShoppingCart> SetShoppingCart(ShoppingCartInputDTO input);
    public Task<List<ShoppingCartDTO>> GetShoppingCartDTOs(string customerNo);
    public Task<bool> UpdateCartSelected(List<ShoppingCartSelectedEditDTO> edits, string customerNo);
    public Task<int> GetShoppingCartNum(string customerNo);
}
