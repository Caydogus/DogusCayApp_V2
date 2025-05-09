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
            var users = await _client.GetFromJsonAsync<List<ResultUserDto>>("users/BolgeMuduruList");
            return View(users);
        }
    }
}
