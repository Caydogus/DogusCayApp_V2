using AutoMapper;
using DogusCay.DTO.DTOs.RoleDtos;
using DogusCay.Entity.Entities;

namespace OnlineEdu.API.Mapping
{
    public class RoleMapping: Profile
    {
        public RoleMapping()
        {
            CreateMap<AppRole, CreateRoleDto>().ReverseMap();
        }
    }
}
