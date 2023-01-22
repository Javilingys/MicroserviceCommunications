using AutoMapper;
using MicroserviceCommunication.Catalog.DTOs;
using MicroserviceCommunication.Catalog.Entities;

namespace MicroserviceCommunication.Catalog.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Because ProductReadDto is immutable record - we need map to consturctore params instead members
            CreateMap<Product, ProductReadDto>()
                .ForCtorParam("Color", opt => opt.MapFrom(src => src.ProductColor.ToString()))
                .ForCtorParam("ProductType", opt => opt.MapFrom(src => src.ProductType.Title))
                .ForCtorParam("ProductBrand", opt => opt.MapFrom(src => src.ProductBrand.Title));

            CreateMap<ProductBrand, ProductBrandReadDto>();
            CreateMap<ProductType, ProductTypeReadDto>();
        }
    }
}
