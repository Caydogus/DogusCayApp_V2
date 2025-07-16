using AutoMapper;
using DogusCay.DTO.DTOs.LoginRegisterDtos;
using DogusCay.DTO.DTOs.RoleDtos;
using DogusCay.DTO.DTOs.UserDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.API.Mapping
{

    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppRole, CreateRoleDto>().ReverseMap();
            CreateMap<AppRole, UpdateRoleDto>().ReverseMap();
            CreateMap<AppUser, ResultUserDto>().ReverseMap();
            CreateMap<AppUser, AdminChangePasswordDto>().ReverseMap();

        }
    }
}

