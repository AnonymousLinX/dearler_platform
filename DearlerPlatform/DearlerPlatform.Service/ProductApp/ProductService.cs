using AutoMapper;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Core.GlobalDTO;
using DearlerPlatform.Service.ProductApp.ProductDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata;
using DearlerPlatform.Core.Core;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Service.ShoppingCartApp.DTO;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.ProductApp;

public partial class ProductService : IProductService
{
    private IRepository<Product> ProductRepo { get; set; }
    private IRepository<ProductSale> ProductSaleRepo { get; set; }
    private IRepository<ProductPhoto> ProductPhotoRepo { get; set; }
    private IRepository<ProductSaleAreaDiff> ProductSaleAreaDiffRepo { get; set; }
    private IMapper Mapper { get; set; }    public ProductService(
        IRepository<Product> productRepo,
        IRepository<ProductSale> productSaleRepo,
        IRepository<ProductPhoto> productPhotoRepo,
        IRepository<ProductSaleAreaDiff> productSaleAreaDiffRepo,
        IMapper mapper,
        LocalEventBus<List<ShoppingCartDTO>> localEventBusShoppingCartDTO
        )
    {
        ProductRepo = productRepo;
        ProductSaleRepo = productSaleRepo;
        ProductPhotoRepo = productPhotoRepo;
        ProductSaleAreaDiffRepo = productSaleAreaDiffRepo;
        Mapper = mapper;
        localEventBusShoppingCartDTO.Subscribe(LocalEventHandler);
    }

    public async Task LocalEventHandler(List<ShoppingCartDTO> dtos)
    {
        var nos = dtos.Select(d => d.ProductNo).ToList();
        var productDtos = await GetProductByProductNos(nos.ToArray());
        var productDtoMap = productDtos.ToDictionary(p => p.ProductNo);
        dtos.ForEach(dto =>
        {
            productDtoMap.TryGetValue(dto.ProductNo, out var productDto);
            dto.ProductDto = productDto;
        });
    }

    public async Task<IEnumerable<ProductDTO>> GetProductDTOsAsync(
        Dictionary<string, List<string>> proptypes,
        string searchText,
        string productType,
        string blongType,
        PageWithSortDTO pageWithSortDTO)
    {
        proptypes.TryGetValue("ProductBzgg", out List<string> bzgg);
        proptypes.TryGetValue("ProductCd", out List<string> cd);
        proptypes.TryGetValue("ProductYs", out List<string> ys);
        proptypes.TryGetValue("ProductGg", out List<string> gg);
        proptypes.TryGetValue("ProductDj", out List<string> dj);
        proptypes.TryGetValue("ProductMc", out List<string> mc);
        proptypes.TryGetValue("ProductHs", out List<string> hs);
        proptypes.TryGetValue("ProductGy", out List<string> gy);
        proptypes.TryGetValue("ProductHd", out List<string> hd);
        proptypes.TryGetValue("ProductPp", out List<string> pp);
        proptypes.TryGetValue("ProductHb", out List<string> hb);
        proptypes.TryGetValue("ProductCz", out List<string> cz);
        proptypes.TryGetValue("ProductXh", out List<string> xh);
        var products = await ProductRepo.GetListAsync(pageWithSortDTO, m => m.BelongTypeNo.ToUpper() == blongType.ToUpper()
        && (string.IsNullOrWhiteSpace(productType) || m.TypeNo == productType)
        && (string.IsNullOrWhiteSpace(searchText) || EF.Functions.Like(m.ProductName, $"%{searchText}%"))
        && (bzgg == null || bzgg.Contains(m.ProductBzgg))
        && (cd == null || cd.Contains(m.ProductCd))
        && (ys == null || ys.Contains(m.ProductYs))
        && (gg == null || gg.Contains(m.ProductGg))
        && (dj == null || dj.Contains(m.ProductDj))
        && (mc == null || mc.Contains(m.ProductMc))
        && (hs == null || hs.Contains(m.ProductHs))
        && (gy == null || gy.Contains(m.ProductGy))
        && (hd == null || hd.Contains(m.ProductHd))
        && (pp == null || pp.Contains(m.ProductPp))
        && (hb == null || hb.Contains(m.ProductHb))
        && (cz == null || cz.Contains(m.ProductCz))
        && (xh == null || xh.Contains(m.ProductXh))
        );
        var dtos = Mapper.Map<List<ProductDTO>>(products);
        Console.WriteLine(products.Select(m => m.ProductNo).ToList());
        Console.WriteLine(products.Select(m => m.ProductNo).ToList());
        var productPhotos = await GetProductPhotosByProductNO([.. products.Select(m => m.ProductNo)]);
        var productSales = await GetProductSalesByProductNO([.. products.Select(m => m.ProductNo)]);
        dtos.ForEach(p =>
        {
            p.ProductPhoto = productPhotos.FirstOrDefault(m => m.ProductNo == p.ProductNo);
            p.ProductSale = productSales.FirstOrDefault(m => m.ProductNo == p.ProductNo);
        });
        return dtos;
    }

    public async Task<IEnumerable<ProductTypeDTO>> GetProductTypes(string belongTypeNo)
    {
        var query =
        from m in ProductRepo.GetQueryable()
        where m.BelongTypeNo == belongTypeNo
        group m by new { m.TypeName, m.TypeNo } into g
        select new ProductTypeDTO
        {
            TypeName = g.Key.TypeName,
            TypeNo = g.Key.TypeNo
        };
        return await query.ToListAsync();
    }

    public async Task<List<BlongTypeDTO>> GetBlongTypeDTOsAsync()
    {
        return await ProductRepo.GetQueryable().Select(m => new BlongTypeDTO
        {
            SystNo = m.SysNo,
            BelongTypeName = m.BelongTypeName,
            BelongTypeNo = m.BelongTypeNo
        }).Distinct().ToListAsync();
    }

    public async Task<Dictionary<string, IEnumerable<string>>> GetProductProps(string belongTypeNo, string typeNo)
    {
        var products = await ProductRepo.GetListAsync(m => m.BelongTypeNo == belongTypeNo && (m.TypeNo == typeNo || string.IsNullOrWhiteSpace(typeNo)));
        return new Dictionary<string, IEnumerable<string>>
        {
            {"ProductXhList | 型号", products.Select(p => p.ProductXh).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductCzList | 材质", products.Select(p => p.ProductCz).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductHbList | 环保", products.Select(p => p.ProductHb).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductPpList | 品牌", products.Select(p => p.ProductPp).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductHdList | 厚度", products.Select(p => p.ProductHd).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductGyList | 工艺", products.Select(p => p.ProductGy).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductHsList | 花色", products.Select(p => p.ProductHs).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductMcList | 面材", products.Select(p => p.ProductMc).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductDjList | 等级", products.Select(p => p.ProductDj).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductCdList | 产地", products.Select(p => p.ProductCd).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductGgList | 规格", products.Select(p => p.ProductGg).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductYsList | 颜色", products.Select(p => p.ProductYs).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
            {"ProductBzggList | 包装规格", products.Select(p => p.ProductBzgg).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList()},
        };
    }

    public async Task<List<ProductDTO>> GetProductByProductNos(params string[] postProductNos)
    {
        var productNos = postProductNos.Distinct().ToArray();
        var products = await ProductRepo.GetListAsync(m => productNos.Contains(m.ProductNo));
        var productDtos = Mapper.Map<List<Product>, List<ProductDTO>>(products);
        var ProductSales = await GetProductSalesByProductNO(productDtos.Select(m => m.ProductNo).ToArray());
        productDtos.ForEach(p =>
        {
            p.ProductSale = ProductSales.FirstOrDefault(m => m.ProductNo == p.ProductNo);
        });
        return productDtos;
    }
}