using DogusCay.WebUI.Areas.Admin.Models;
using DogusCay.WebUI.DTOs.PivotDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class PivotController : Controller
    {
        private readonly HttpClient _client;

        public PivotController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("EduClient");
        }

        public IActionResult Index()
        {
            // UI tablosu seçimi için liste
            var model = new PivotViewModel
            {
                Tables = new List<string>
                {
                    "VW_CIRO_OZETI_2025",
                    "VW_CIRO_OZETI_2025_MUSTERIGRUPAD",
                    "VW_AylaraGoreDogusKatilim",
                    "VW_AylaraGoreDistKatilim",
                    "VW_AylaraGoreButce",
                    "VW_AylaraGoreBayideKalan",
                    "VW_CIRO_OZETI_2025_GRUPLU",
                    "VW_CiroOzetiGrupAdaGore",
                    "CIRO_OZETI_2025",
                    "FATURA_OZET_TEMP"
                }
            };

            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> LoadPivotData([FromBody] PivotRequest request)
        {
            var jwt = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(jwt))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwt);
            }

            // API çağır
            var apiResponse = await _client.PostAsJsonAsync("pivots/get", request);

            var raw = await apiResponse.Content.ReadAsStringAsync();
            Console.WriteLine("API RAW RESPONSE: " + raw);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)apiResponse.StatusCode, raw);
            }

            try
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(raw);
                return Json(data);
            }
            catch
            {
                return StatusCode(500, "JSON parse hatası: " + raw);
            }
        }
    }
}
