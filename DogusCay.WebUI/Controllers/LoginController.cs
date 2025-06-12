using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DogusCay.WebUI.DTOs.LoginDtos;
using DogusCay.WebUI.DTOs.UserDtos;
using DogusCay.WebUI.Services.TokenServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;
        private readonly ITokenService _tokenService;

        public LoginController(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _client = clientFactory.CreateClient("EduClient");
            _tokenService = tokenService;
        }

        public IActionResult SignIn() => View();

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginDto userLoginDto)
        {
            var result = await _client.PostAsJsonAsync("users/login", userLoginDto);
            if (!result.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı");
                return View(userLoginDto);
            }

            var response = await result.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (response is null || string.IsNullOrEmpty(response.Token))
            {
                ModelState.AddModelError("", "Token alınamadı.");
                return View(userLoginDto);
            }

            // 🔐 Token'ı session'a yaz
            HttpContext.Session.SetString("JwtToken", response.Token);

            // 🔐 Token'dan claim'leri oku
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(response.Token);
            var claims = token.Claims.ToList();

            // 🔐 Kimliği oluştur ve login yap
            var identity = new ClaimsIdentity(claims, "DogusCookie");
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = response.ExpireDate
            };

            await HttpContext.SignInAsync("DogusCookie", new ClaimsPrincipal(identity), authProps);

            return RedirectToAction("Index", "TalepForm", new { area = "Admin" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("DogusCookie");
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}

