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

//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using DogusCay.WebUI.DTOs.LoginDtos; // UserLoginDto ve LoginResponseDto için
//using DogusCay.WebUI.DTOs.UserDtos;
//using DogusCay.WebUI.Services.TokenServices; // ITokenService için
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Mvc;

//namespace DogusCay.WebUI.Controllers
//{
//    public class LoginController : Controller
//    {
//        private readonly HttpClient _client;
//        private readonly ITokenService _tokenService; // Yeni eklendi: TokenService'i kullanmak için

//        public LoginController(IHttpClientFactory clientFactory, ITokenService tokenService) // Constructor güncellendi
//        {
//            _client = clientFactory.CreateClient("EduClient");
//            _tokenService = tokenService; // TokenService'i atama
//        }

//        public IActionResult SignIn()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> SignIn(UserLoginDto userLoginDto)
//        {
//            var result = await _client.PostAsJsonAsync("users/login", userLoginDto);
//            if (!result.IsSuccessStatusCode)
//            {
//                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı");
//                return View(userLoginDto);
//            }

//            var handler = new JwtSecurityTokenHandler();
//            var response = await result.Content.ReadFromJsonAsync<LoginResponseDto>();

//            // **API'den gelen token'ı ITokenService aracılığıyla Session'a kaydedin**
//            if (!string.IsNullOrEmpty(response.Token))
//            {
//                _tokenService.SetUserToken(response.Token);
//                TempData["JwtToken"] = response.Token;
//            }
//            else
//            {
//                ModelState.AddModelError("", "Giriş başarılı ancak token alınamadı.");
//                return View(userLoginDto);
//            }

//            var token = handler.ReadJwtToken(response.Token);
//            var claims = token.Claims.ToList();

//            // Orijinal token string'ini claim olarak eklemeye artık gerek yok, Session'da saklanıyor.
//            // claims.Add(new Claim("Token", response.Token)); // Bu satırı kaldırın

//            var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
//            var authProps = new AuthenticationProperties
//            {
//                ExpiresUtc = response.ExpireDate,
//                IsPersistent = true
//            };

//            await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);

//            return RedirectToAction("Index", "TalepForm", new { area = "Admin" });

//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync();
//            _tokenService.ClearUserToken();
//            return RedirectToAction("SignIn", "Login");
//        }

//    }
//}