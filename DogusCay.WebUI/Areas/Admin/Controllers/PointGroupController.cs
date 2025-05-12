using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class PointGroupController : Controller
    {
        private readonly HttpClient _client;

        public PointGroupController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }

        //nokta gruplarını kanallarıyla beraber getir
        public async Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultPointGroupDto>>("pointGroups/with-kanal");
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePointGroup()
        {

            var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            ViewBag.Kanals = new SelectList(kanalList,"KanalId","KanalName");

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreatePointGroup(CreatePointGroupDto createPointGroupDto)
        {
            // Eğer model valid değilse (KanalId seçilmemiş, Grup Adı boş vs.)
            if (!ModelState.IsValid)
            {
                var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
                ViewBag.Kanals = new SelectList(kanalList, "KanalId", "KanalName");
                return View(createPointGroupDto);
            }

            // API'ye post işlemi
            var response = await _client.PostAsJsonAsync("pointgroups", createPointGroupDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Eğer API'den hata dönerse (örnek: validation fail, 500, vs.)
            var kanalRetry = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            ViewBag.Kanals = new SelectList(kanalRetry, "KanalId", "KanalName");

            // API hata mesajı eklenebilir:
            ModelState.AddModelError("", "Sunucuda bir hata oluştu. Lütfen tekrar deneyin.");

            return View(createPointGroupDto);
        }

    }
}