using AutoMapper;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp.ProductDTOs;
using DearlerPlatform.Service.ShoppingCartApp.DTO;

namespace DearlerPlatform.Service;

public class DearlerPlatformProfile: Profile
{
    public DearlerPlatformProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Product, ProductPhoto>().ReverseMap();
        CreateMap<Product, ProductSale>().ReverseMap();
        CreateMap<Product, ProductSaleAreaDiff>().ReverseMap();
        CreateMap<ShoppingCart, ShoppingCartInputDTO>().ReverseMap();
        CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
    }
}
