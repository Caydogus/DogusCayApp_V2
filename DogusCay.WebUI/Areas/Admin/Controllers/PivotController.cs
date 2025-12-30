//using DogusCay.WebUI.Areas.Admin.Models;
//using DogusCay.WebUI.DTOs.PivotDtos;
//using Microsoft.AspNetCore.Mvc;
//using System.Configuration;
//using System.Net.Http.Headers;

//namespace DogusCay.WebUI.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Route("admin/[controller]/[action]/{id?}")]
//    public class PivotController : Controller
//    {
//        private readonly HttpClient _client;
//        private readonly IConfiguration _configuration;

//        public PivotController(IHttpClientFactory factory, IConfiguration configuration)
//        {
//            _client = factory.CreateClient("EduClient");
//            _configuration = configuration;
//        }

//        public IActionResult Index()
//        {
//            var tables = _configuration
//                .GetSection("PivotSettings:Tables")
//                .Get<List<string>>() ?? new List<string>();

//            var model = new PivotViewModel
//            {
//                Tables = tables
//            };


//            return View(model);
//        }


//        [HttpPost]
//        public async Task<IActionResult> LoadPivotData([FromBody] PivotRequest request)
//        {
//            var jwt = HttpContext.Session.GetString("JwtToken");

//            if (!string.IsNullOrEmpty(jwt))
//            {
//                _client.DefaultRequestHeaders.Authorization =
//                    new AuthenticationHeaderValue("Bearer", jwt);
//            }

//            // API çağır
//            var apiResponse = await _client.PostAsJsonAsync("pivots/get", request);

//            var raw = await apiResponse.Content.ReadAsStringAsync();
//            Console.WriteLine("API RAW RESPONSE: " + raw);

//            if (!apiResponse.IsSuccessStatusCode)
//            {
//                return StatusCode((int)apiResponse.StatusCode, raw);
//            }

//            try
//            {
//                var data = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(raw);
//                return Json(data);
//            }
//            catch
//            {
//                return StatusCode(500, "JSON parse hatası: " + raw);
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> ExportToExcel(string tableName)
//        {
//            if (string.IsNullOrWhiteSpace(tableName))
//            {
//                TempData["Error"] = "Excel için önce bir pivot tablosu seçmelisiniz.";
//                return RedirectToAction("Index");
//            }

//            var token = HttpContext.Session.GetString("JwtToken");
//            if (string.IsNullOrEmpty(token))
//            {
//                TempData["Error"] = "Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.";
//                return RedirectToAction("Index");
//            }

//            // API endpoint: /api/pivots/export-excel?tableName=...
//            var request = new HttpRequestMessage(
//                HttpMethod.Get,
//                $"pivots/export-excel?tableName={Uri.EscapeDataString(tableName)}"
//            );

//            request.Headers.Authorization =
//                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

//            var response = await _client.SendAsync(request);

//            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
//            {
//                TempData["Error"] = "İndirilecek kayıt bulunamadı.";
//                return RedirectToAction("Index");
//            }

//            if (!response.IsSuccessStatusCode)
//            {
//                var error = await response.Content.ReadAsStringAsync();
//                TempData["Error"] = "Excel indirme başarısız: " + error;
//                return RedirectToAction("Index");
//            }

//            var content = await response.Content.ReadAsByteArrayAsync();
//            var fileName = $"{tableName}_{DateTime.Now:yyyyMMdd}.xlsx";

//            return File(
//                content,
//                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                fileName
//            );
//        }
//    }
//}


using DogusCay.WebUI.Areas.Admin.Models;
using DogusCay.WebUI.DTOs.PivotDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class PivotController : Controller
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public PivotController(IHttpClientFactory factory, IConfiguration configuration)
        {
            _client = factory.CreateClient("EduClient");
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // appsettings.json -> PivotSettings:Tables listesini oku
            var tables = _configuration
                .GetSection("PivotSettings:Tables")
                .Get<List<string>>() ?? new List<string>();

            var model = new PivotViewModel
            {
                Tables = tables
            };

            return View(model);
        }

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
                var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                    raw,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return Json(data);
            }
            catch
            {
                return StatusCode(500, "JSON parse hatası: " + raw);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                TempData["Error"] = "Excel için önce bir pivot tablosu seçmelisiniz.";
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.";
                return RedirectToAction("Index");
            }

            // API endpoint: /api/pivots/export-excel?tableName=...
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"pivots/export-excel?tableName={Uri.EscapeDataString(tableName)}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                TempData["Error"] = "İndirilecek kayıt bulunamadı.";
                return RedirectToAction("Index");
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Excel indirme başarısız: " + error;
                return RedirectToAction("Index");
            }

            var content = await response.Content.ReadAsByteArrayAsync();
            var fileName = $"{tableName}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
