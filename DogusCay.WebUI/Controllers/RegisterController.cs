using System.Text.Json;
using DogusCay.WebUI.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegisterController : Controller
    {
        private readonly HttpClient _client;

        public RegisterController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserRegisterDto userRegisterDto)
        {
            var result = await _client.PostAsJsonAsync("users/register", userRegisterDto);

            if (!ModelState.IsValid)
            {
                return View(userRegisterDto);
            }

            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                try
                {
                    var errors = JsonSerializer.Deserialize<List<RegisterResponseDto>>(content);
                    foreach (var item in errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                catch
                {
                    // Gelen içerik JSON formatında değilse direkt mesajı göster
                    ModelState.AddModelError("", content);
                }

                return View(userRegisterDto);
            }

            //return RedirectToAction("SignIn", "Login");
            return RedirectToAction("Index", "Director", new { area = "Admin" });

        }

    }
}
