using DogusCay.DTO.DTOs.LoginRegisterDtos;
using DogusCay.DTO.DTOs.UserDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Identity;

namespace DogusCay.Business.Abstract
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterDto userRegisterDto);

        Task<string> LoginAsync(LoginDto userLoginDto);
        Task LogoutAsync();

        Task<bool> CreateRoleAsync(UserRoleDto userRoleDto);

        Task<bool> AssignRoleAsync(List<AssignRoleDto> assignRoleDto);
        Task<List<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);

    }
}
