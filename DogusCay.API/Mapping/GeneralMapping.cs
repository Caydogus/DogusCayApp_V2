using AutoMapper;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.DTO.DTOs.ChannelDtos;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.DTO.DTOs.PointGrupDtos;
using DogusCay.DTO.DTOs.ProductDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.API.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIdCategoryDto>().ReverseMap(); 
            
            CreateMap<Product, ResultProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetByIdProductDto>().ReverseMap();

            CreateMap<Kanal, ResultKanalDto>().ReverseMap();
            CreateMap<Kanal, CreateKanalDto>().ReverseMap();
            CreateMap<Kanal, UpdateKanalDto>().ReverseMap();
            CreateMap<Kanal, GetByIdKanalDto>().ReverseMap();

            CreateMap<Point, ResultPointDto>().ReverseMap();
            CreateMap<Point, CreatePointDto>().ReverseMap();
            CreateMap<Point, UpdatePointDto>().ReverseMap();
            CreateMap<Point, GetByIdPointDto>().ReverseMap();

            CreateMap<PointGroup, ResultPointGroupDto>().ReverseMap();
            CreateMap<PointGroup, CreatePointGroupDto>().ReverseMap();
            CreateMap<PointGroup, UpdatePointGroupDto>().ReverseMap();
            CreateMap<PointGroup, GetByIdPointGroupDto>().ReverseMap();
        }
    }
}
