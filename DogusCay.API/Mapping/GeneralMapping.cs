using AutoMapper;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.DTO.DTOs.ChannelDtos;
using DogusCay.DTO.DTOs.PaymentTypeDtos;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.DTO.DTOs.PointGrupDtos;
using DogusCay.DTO.DTOs.ProductDtos;
using DogusCay.DTO.DTOs.RegionDtos;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.DTOs.ProductDtos;
using DogusCay.Entity.Entities;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.API.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // CATEGORY ↔ DTO
            CreateMap<Category, ResultCategoryDto>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIdCategoryDto>().ReverseMap();

            // PRODUCT ↔ DTO
            CreateMap<Product, ResultProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.UnitTypeName, opt => opt.MapFrom(src => src.UnitType.UnitTypeName))
                .ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetByIdProductDto>().ReverseMap();

            // KANAL ↔ DTO
            CreateMap<Kanal, ResultKanalDto>().ReverseMap();
            CreateMap<Kanal, CreateKanalDto>().ReverseMap();
            CreateMap<Kanal, UpdateKanalDto>().ReverseMap();
            CreateMap<Kanal, GetByIdKanalDto>().ReverseMap();

            // POINT ↔ DTO
            CreateMap<Point, ResultPointDto>().ReverseMap();
            CreateMap<Point, CreatePointDto>().ReverseMap();
            CreateMap<Point, UpdatePointDto>().ReverseMap();
            CreateMap<Point, GetByIdPointDto>().ReverseMap();

            // POINT GROUP ↔ DTO
            CreateMap<PointGroup, ResultPointGroupDto>().ReverseMap();
            CreateMap<PointGroup, CreatePointGroupDto>().ReverseMap();
            CreateMap<PointGroup, UpdatePointGroupDto>().ReverseMap();
            CreateMap<PointGroup, GetByIdPointGroupDto>().ReverseMap();

            // PAYMENT TYPE ↔ DTO
            CreateMap<PaymentType, ResultPaymentTypeDto>().ReverseMap();
            CreateMap<PaymentType, CreatePaymentTypeDto>().ReverseMap();
            CreateMap<PaymentType, UpdatePaymentTypeDto>().ReverseMap();

            // REGION ↔ DTO
            CreateMap<Region, ResultRegionDto>()
                .ForMember(dest => dest.ManagerFirstName, opt => opt.MapFrom(src => src.ManagerUser.FirstName))
                .ForMember(dest => dest.ManagerLastName, opt => opt.MapFrom(src => src.ManagerUser.LastName));
            CreateMap<Region, CreateRegionDto>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();

            // TALEP FORM ↔ DTO
            CreateMap<TalepForm, CreateTalepFormDto>().ReverseMap();
            CreateMap<TalepForm, UpdateTalepFormDto>().ReverseMap();
            CreateMap<TalepFormItem, CreateTalepFormItemDto>().ReverseMap();
            CreateMap<TalepFormItem, UpdateTalepFormItemDto>().ReverseMap();
        }
    }
}
