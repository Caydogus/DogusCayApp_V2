using DogusCay.WebUI.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class DirectorController : Controller
    {
        private readonly HttpClient _client;

        public DirectorController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("EduClient");
        }

        public async Task<IActionResult> Index()
        {
            var users = await _client.GetFromJsonAsync<List<ResultUserDto>>("users");
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminChangePassword(AdminChangePasswordDto dto)
        {
            // JWT token ekle (Admin olarak giriş yapılmış olmalı)
            var jwt = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(jwt))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            }

            var response = await _client.PostAsJsonAsync("users/admin-change-password", dto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = await response.Content.ReadAsStringAsync();
            }
            else
            {
                TempData["Success"] = "Şifre başarıyla değiştirildi.";
            }
            return RedirectToAction("Index");
        }
    }
}





//using DogusCay.WebUI.DTOs.UserDtos;
//using Microsoft.AspNetCore.Mvc;

//namespace DogusCay.WebUI.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Route("[area]/[controller]/[action]/{id?}")]
//    public class DirectorController : Controller
//    {
//        private readonly HttpClient _client;

//        public DirectorController(IHttpClientFactory factory)
//        {
//            _client = factory.CreateClient("EduClient");
//        }

//        public async Task<IActionResult> Index()
//        {
//            var users = await _client.GetFromJsonAsync<List<ResultUserDto>>("users/BolgeMuduruList");
//            return View(users);
//        }
//    }
//}
