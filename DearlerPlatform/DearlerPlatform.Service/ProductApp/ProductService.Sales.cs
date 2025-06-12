using DearlerPlatform.Domain;

namespace DearlerPlatform.Service.ProductApp;

public partial class ProductService
{
    public async Task<List<ProductSale>> GetProductSalesByProductNO(params string[] productNos)
    {
        return await ProductSaleRepo.GetListAsync(m => productNos.Contains(m.ProductNo));
    }
}
