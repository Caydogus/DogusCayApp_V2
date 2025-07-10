using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.LoginRegisterDtos;
using DogusCay.DTO.DTOs.UserDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IJwtService _jwtService, IMapper _mapper) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Bu Email Sistemde Kayıtlı Değil");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest("Kullanıcı Adı veya Şifre Hatalı");
            }

            var token = await _jwtService.CreateTokenAsync(user);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = _mapper.Map<AppUser>(model);

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await _userManager.AddToRoleAsync(user, "BolgeMuduru");//kayıt yaparken otomatik olarak bolge mudur olarak kaydediyor
                return Ok("Kullanıcı Kaydı Başarılı");
            }


            return BadRequest();
        }

     
        [HttpGet("BolgeMuduruList")]
        public async Task<IActionResult> BolgeMuduruList()
        {
            var users = await _userManager.GetUsersInRoleAsync("BolgeMuduru");

            var result = new List<ResultUserDto>();

            foreach (var user in users)
            {
                result.Add(new ResultUserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = user.ImageUrl,
                    Roles = new List<string> { "BolgeMuduru" }
                });
            }

            return Ok(result);
        }

        [HttpGet("SatisTemsilcisiList")]
        public async Task<IActionResult> SatisTemsilcisiList()
        {
            var users = await _userManager.GetUsersInRoleAsync("SatisTemsilcisi");

            var result = new List<ResultUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new ResultUserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = user.ImageUrl,
                    Roles = roles
                });
            }

            return Ok(result);
        }
    }
}
