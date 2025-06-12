using DearlerPlatform.Core.GlobalDTO;
using DearlerPlatform.Service.ProductApp.ProductDTOs;

namespace DearlerPlatform.Service.ProductApp;

public interface IProductService
{
    public Task<IEnumerable<ProductTypeDTO>> GetProductTypes(string belongTypeNo);
    public Task<IEnumerable<ProductDTO>> GetProductDTOsAsync(Dictionary<string, List<string>> proptypes, string searchText, string productType, string blongType, PageWithSortDTO pageWithSortDTO);
    public Task<Dictionary<string, IEnumerable<string>>> GetProductProps(string belongTypeNo, string typeNo);
    public Task<List<BlongTypeDTO>> GetBlongTypeDTOsAsync();
}