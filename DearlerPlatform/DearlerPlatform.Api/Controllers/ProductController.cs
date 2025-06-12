using DearlerPlatform.Api.Tools;
using DearlerPlatform.Core.GlobalDTO;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ProductApp.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Api.Controllers;

// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ProductController : BaseController
{
    public IProductService ProductService { get; set; }
    public ProductController(IProductService productService)
    {
        ProductService = productService;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductDTO>> GetProductDTOsAsync(string? searchText,
        string? productType,
        string? sort,
        int pageIndex,
        int pageSize,
        string blongType = "WJ",
        OrderType orderType = OrderType.Asc,
        string? proptype = null)
    {
        var proptypes = proptype.UrlEncodedStringToListDictionary();
        searchText ??= "";
        productType ??= "";
        if (pageIndex <= 0) pageIndex = 1;
        if (pageSize <= 0) pageSize = 5;
        sort = string.IsNullOrEmpty(sort) ? "ProductName" : sort;
        return await ProductService.GetProductDTOsAsync(proptypes, searchText, productType, blongType, new PageWithSortDTO
        {
            Sort = sort,
            PageIndex = pageIndex,
            PageSize = pageSize,
            OrderType = orderType
        });
    }

    [HttpGet("type")]
    public async Task<IEnumerable<ProductTypeDTO>> GetProductTypeDTOAsync(string belongTypeNo)
    {
        return await ProductService.GetProductTypes(belongTypeNo);
    }

    [HttpGet("props")]
    public async Task<Dictionary<string, IEnumerable<string>>> GetProductProps(string? typeNo, string belongTypeNo = "WJ")
    {
        return await ProductService.GetProductProps(belongTypeNo, typeNo);
    }

    [HttpGet("BlongType")]
    public async Task<List<BlongTypeDTO>> GetBlongType()
    {
        return await ProductService.GetBlongTypeDTOsAsync();
    }
} 
