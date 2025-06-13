using Newtonsoft.Json;
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
        CreateMap<ProductDTO, ProductCTO>()
        .ForMember(dest => dest.ProductPhoto, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ProductPhoto)))
        .ForMember(dest => dest.ProductSale, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ProductSale)));
        CreateMap<ProductCTO, ProductDTO>()
        .ForMember(dest => dest.ProductPhoto, opt => opt.MapFrom(src => JsonConvert.DeserializeObject(src.ProductPhoto)))
        .ForMember(dest => dest.ProductSale, opt => opt.MapFrom(src => JsonConvert.DeserializeObject(src.ProductSale)));
    }
}
