using DearlerPlatform.Service.ProductApp.ProductDTOs;

namespace DearlerPlatform.Service.ShoppingCartApp.DTO;

public class ShoppingCartDTO
{
    public new int Id { get; set; }

    public string CartGuid { get; set; } = null!;

    public string CustomerNo { get; set; } = null!;

    public string ProductNo { get; set; } = null!;

    public int ProductNum { get; set; }

    public bool CartSelected { get; set; }

    public ProductDTO ProductDto { get; set; }
}
