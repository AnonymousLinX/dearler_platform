using DearlerPlatform.Domain;

namespace DearlerPlatform.Service.ProductApp;

public partial class ProductService
{
    public async Task<List<ProductPhoto>> GetProductPhotosByProductNO(params string[] productNos)
    {
        return await ProductPhotoRepo.GetListAsync(m => productNos.Contains(m.ProductNo));
    }
}
