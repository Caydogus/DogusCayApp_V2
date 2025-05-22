using AutoMapper;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.DTO.DTOs.ChannelDtos;
using DogusCay.DTO.DTOs.DistributorDtos;
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
            CreateMap<PointGroupType, ResultPointGroupTypeDto>().ReverseMap();
            CreateMap<PointGroupType, CreatePointGroupTypeDto>().ReverseMap();
            CreateMap<PointGroupType, UpdatePointGroupTypeDto>().ReverseMap();

            // PAYMENT TYPE ↔ DTO
            CreateMap<PaymentType, ResultPaymentTypeDto>().ReverseMap();
            CreateMap<PaymentType, CreatePaymentTypeDto>().ReverseMap();
            CreateMap<PaymentType, UpdatePaymentTypeDto>().ReverseMap();
           
            CreateMap<Distributor, CreateDistributorDto>().ReverseMap();
            CreateMap<Distributor, UpdateDistributorDto>().ReverseMap();
            CreateMap<Distributor, ResultDistributorDto>().ReverseMap();
            CreateMap<AppUser,     SimpleUserDto>().ReverseMap(); ;

            // Create DTO -> Entity
            CreateMap<CreateTalepFormDto, TalepForm>();
            CreateMap<CreateTalepFormItemDto, TalepFormItem>();

            // UPDATE
            CreateMap<UpdateTalepFormDto, TalepForm>();
            CreateMap<UpdateTalepFormItemDto, TalepFormItem>();

            // RESULT (Form + Item)
            CreateMap<TalepForm, ResultTalepFormDto>()
                .ForMember(dest => dest.TalepTipName, opt => opt.MapFrom(src => src.TalepTip.ToString()))
                .ForMember(dest => dest.TalepDurumuText, opt => opt.MapFrom(src => src.TalepDurumu.ToString()))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.AppUser.FirstName + " " + src.AppUser.LastName))
                .ForMember(dest => dest.KanalName, opt => opt.MapFrom(src => src.Kanal.KanalName))
                .ForMember(dest => dest.DistributorName, opt => opt.MapFrom(src => src.Distributor.DistributorName))
                .ForMember(dest => dest.PointName, opt => opt.MapFrom(src => src.Point.PointName))
                .ForMember(dest => dest.PointGroupTypeName, opt => opt.MapFrom(src => src.PointGroupType.PointGroupTypeName))
                
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.TalepFormItems));
             CreateMap<CreateTalepFormDto, TalepForm>()
                .ForMember(dest => dest.TalepFormItems, opt => opt.MapFrom(src => src.Items));
             CreateMap<TalepFormItem, ResultTalepFormItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.CategoryName));

        }
    }
}
