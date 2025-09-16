using AutoMapper;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.DTO.DTOs.DistributorDtos;
using DogusCay.DTO.DTOs.KanalDtos;
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.DTO.DTOs.PaymentTypeDtos;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.DTO.DTOs.PointGrupDtos;
using DogusCay.DTO.DTOs.ProductDtos;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.DTOs.ProductDtos;
using DogusCay.Entity.Entities;
using DogusCay.Entity.Entities.MalYuklemeTalep;

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
     
            CreateMap<Product, ResultProductDto>()
     .ForMember(dest => dest.CategoryName,
         opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
     .ForMember(dest => dest.ParentCategoryName,
         opt => opt.MapFrom(src => src.Category != null && src.Category.ParentCategory != null
             ? src.Category.ParentCategory.CategoryName
             : null))
     .ForMember(dest => dest.UnitTypeName,
         opt => opt.MapFrom(src => src.UnitType != null ? src.UnitType.UnitTypeName : null))
     .ForMember(dest => dest.OneriRafFiyati,
         opt => opt.MapFrom(src => src.OneriRafFiyati))   
     .ForMember(dest => dest.OneriAksiyonFiyati,
         opt => opt.MapFrom(src => src.OneriAksiyonFiyati)); 

            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetByIdProductDto>().ReverseMap();

            // KANAL DTO
            CreateMap<Kanal, ResultKanalDto>().ReverseMap();
            CreateMap<Kanal, CreateKanalDto>().ReverseMap();
            CreateMap<Kanal, UpdateKanalDto>().ReverseMap();
            CreateMap<Kanal, GetByIdKanalDto>().ReverseMap();

            // POINT  DTO
            CreateMap<Point, ResultPointDto>().ReverseMap();
            CreateMap<Point, CreatePointDto>().ReverseMap();
            CreateMap<Point, UpdatePointDto>().ReverseMap();
            CreateMap<Point, GetByIdPointDto>().ReverseMap();

            // POINT GROUP  DTO
            CreateMap<PointGroupType, ResultPointGroupTypeDto>().ReverseMap();
            CreateMap<PointGroupType, CreatePointGroupTypeDto>().ReverseMap();
            CreateMap<PointGroupType, UpdatePointGroupTypeDto>().ReverseMap();

            // PAYMENT TYPE  DTO
            CreateMap<PaymentType, ResultPaymentTypeDto>().ReverseMap();
            CreateMap<PaymentType, CreatePaymentTypeDto>().ReverseMap();
            CreateMap<PaymentType, UpdatePaymentTypeDto>().ReverseMap();
           
            CreateMap<Distributor, CreateDistributorDto>().ReverseMap();
            CreateMap<Distributor, UpdateDistributorDto>().ReverseMap();
            CreateMap<Distributor, ResultDistributorDto>().ReverseMap();
            CreateMap<AppUser,     SimpleUserDto>().ReverseMap(); ;

            // Create DTO  Entity
            CreateMap<CreateTalepFormDto, TalepForm>().ReverseMap();

            // UPDATE
            CreateMap<UpdateTalepFormDto, TalepForm>().ReverseMap();

            CreateMap<TalepForm, ResultTalepFormDto>()
              .ForMember(dest => dest.KanalName, opt => opt.MapFrom(src => src.Kanal.KanalName))
              .ForMember(dest => dest.DistributorName, opt => opt.MapFrom(src => src.Distributor.DistributorName))
              .ForMember(dest => dest.PointGroupTypeName, opt => opt.MapFrom(src => src.PointGroupType.PointGroupTypeName))
              .ForMember(dest => dest.PointName, opt => opt.MapFrom(src => src.Point.PointName))
              .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.AppUser.LastName + " " + src.AppUser.FirstName))

              //  Kategori isimleri
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
              .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.CategoryName))
              .ForMember(dest => dest.SubSubCategoryName, opt => opt.MapFrom(src => src.SubSubCategory.CategoryName));

            // TalepFormItem  ResultTalepFormItemDto

            CreateMap<CreateTalepFormDto, TalepForm>().ReverseMap();

            // Create DTO  Entity
            CreateMap<CreateMalYuklemeTalepFormDto, MalYuklemeTalepForm>().ReverseMap();
            CreateMap<CreateMalYuklemeTalepFormDetailDto, MalYuklemeTalepFormDetail>().ReverseMap();

            CreateMap<MalYuklemeTalepForm, ResultMalYuklemeTalepFormDto>().ReverseMap(); 
            CreateMap<MalYuklemeTalepFormDetail, ResultMalYuklemeTalepFormDetailDto>().ReverseMap(); 

        }
    }
}
