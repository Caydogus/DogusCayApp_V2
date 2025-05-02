using Microsoft.AspNetCore.Identity;
using DogusCay.WebUI.DTOs.UserDtos;
using DogusCay.WebUI.Models;
using DogusCay.Entity.Entities;
using AutoMapper;

namespace DogusCay.WebUI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }
        public Task<bool> AssignRoleAsync(List<AssignRoleDto> assignRoleDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRoleAsync(UserRoleDto userRoleDto)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateUserAsync(UserRegisterDto userRegisterDto)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
