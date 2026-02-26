//using DogusCay.WebUI.DTOs.TalepDtos;
//using DogusCay.WebUI.DTOs.KanalDtos;
//using DogusCay.WebUI.DTOs.ProductDtos;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace DogusCay.WebUI.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Route("admin/[controller]/[action]/{id?}")]
//    public class TalepFormController : Controller
//    {
//        private readonly HttpClient _client;

//        public TalepFormController(IHttpClientFactory httpClientFactory)
//        {
//            _client = httpClientFactory.CreateClient("EduClient");
//        }

//        private async Task SetDropdownsAsync()
//        {
//            var jwtToken = HttpContext.Session.GetString("JwtToken");

//            if (string.IsNullOrEmpty(jwtToken))
//            {
//                Console.WriteLine("Uyarı: Session'dan JWT token alınamadı!");
//                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
//                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
//                TempData["Error"] = "Dropdown verileri yüklenirken bir hata oluştu: Oturum süresi dolmuş veya token bulunamadı.";
//                return;
//            }

//            _client.DefaultRequestHeaders.Clear();
//            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

//            try
//            {
//                var kanallar = await _client.GetFromJsonAsync<List<KanalDropdownDto>>("kanals/dropdown");
//                ViewBag.Kanallar = new SelectList(kanallar, "KanalId", "KanalName");

//                var urunler = await _client.GetFromJsonAsync<List<ProductDropdownDto>>("products/dropdown");
//                ViewBag.Products = new SelectList(urunler, "ProductId", "ProductName");
//            }
//            catch (HttpRequestException httpEx)
//            {
//                Console.WriteLine($"❌ HTTP Request Error (SetDropdownsAsync): {httpEx.Message}");
//                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
//                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
//                TempData["Error"] = $"Dropdown verileri yüklenirken bir HTTP hatası oluştu: {httpEx.Message}";
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Error fetching dropdown data (SetDropdownsAsync): {ex.Message}");
//                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
//                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
//                TempData["Error"] = "Dropdown verileri yüklenirken beklenmeyen bir hata oluştu: " + ex.Message;
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(int page = 1)
//        {
//            try
//            {
//                string endpoint;
//                int pageSize = 10;

//                if (User.IsInRole("Admin"))
//                    endpoint = $"talepforms/paged?page={page}&pageSize={pageSize}";
//                else
//                    endpoint = $"talepforms/mine-paged?page={page}&pageSize={pageSize}";

//                var response = await _client.GetFromJsonAsync<PagedTalepFormResponse>(endpoint);

//                ViewBag.CurrentPage = response.Page;
//                ViewBag.TotalPages = (int)Math.Ceiling((double)response.TotalCount / response.PageSize);
//                ViewBag.TotalCount = response.TotalCount;
//                ViewBag.PageSize = response.PageSize;

//                return View(response.Data);
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = "Talep listesi alınamadı: " + ex.Message;
//                return View(new List<ResultTalepFormDto>());
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> CreateTalepForm()
//        {
//            await SetDropdownsAsync();
//            return View(new CreateTalepFormDto());
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateTalepForm(CreateTalepFormDto createTalepFormDto)
//        {
//            Console.WriteLine("METODA GELDİ");

//            if (!ModelState.IsValid)
//            {
//                await SetDropdownsAsync();
//                return View(createTalepFormDto);
//            }

//            try
//            {
//                var json = System.Text.Json.JsonSerializer.Serialize(createTalepFormDto);
//                Console.WriteLine("Gönderilen JSON:");
//                Console.WriteLine(json);

//                var response = await _client.PostAsJsonAsync("talepforms", createTalepFormDto);
//                var responseText = await response.Content.ReadAsStringAsync();

//                Console.WriteLine("POST Status: " + response.StatusCode);
//                Console.WriteLine("API Yanıtı: " + responseText);

//                if (response.IsSuccessStatusCode)
//                    return RedirectToAction("Index");

//                TempData["error"] = "Talep kaydedilemedi.";
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("❌ HATA: " + ex.Message);
//                TempData["error"] = "İstek gönderilirken hata oluştu: " + ex.Message;
//            }

//            await SetDropdownsAsync();
//            return View(createTalepFormDto);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var response = await _client.DeleteAsync($"talepforms/{id}");

//                if (response.IsSuccessStatusCode)
//                {
//                    TempData["Success"] = "Talep başarıyla silindi.";
//                }
//                else
//                {
//                    TempData["Error"] = "Talep silinirken bir hata oluştu.";
//                }
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = "Hata: " + ex.Message;
//            }

//            return RedirectToAction("Index");
//        }

//        public async Task<IActionResult> ExportToExcel()
//        {
//            var response = await _client.GetAsync("talepforms/export-excel");

//            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
//            {
//                TempData["ExcelError"] = "İndirilecek veri bulunamadı.";
//                return RedirectToAction("Index");
//            }

//            if (!response.IsSuccessStatusCode)
//            {
//                TempData["ExcelError"] = "Excel dosyası alınamadı.";
//                return RedirectToAction("Index");
//            }

//            var content = await response.Content.ReadAsByteArrayAsync();
//            var fileName = "TalepForms.xlsx";

//            return File(content,
//                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                fileName);
//        }


//    }
//}

using DogusCay.WebUI.DTOs.TalepDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class TalepFormController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TalepFormController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateAuthorizedClient()
        {
            var client = _httpClientFactory.CreateClient("EduClient");

            var jwtToken = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(jwtToken))
                throw new Exception("Oturum süresi dolmuş. Lütfen tekrar giriş yapın.");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);

            return client;
        }

        private async Task SetDropdownsAsync()
        {
            try
            {
                var client = CreateAuthorizedClient();

                var kanallar = await client.GetFromJsonAsync<List<KanalDropdownDto>>("kanals/dropdown");
                ViewBag.Kanallar = new SelectList(kanallar ?? new(), "KanalId", "KanalName");

                var urunler = await client.GetFromJsonAsync<List<ProductDropdownDto>>("products/dropdown");
                ViewBag.Products = new SelectList(urunler ?? new(), "ProductId", "ProductName");
            }
            catch (Exception ex)
            {
                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
                TempData["Error"] = "Dropdown verileri alınamadı: " + ex.Message;
            }
        }

        // ================================
        // INDEX
        // ================================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var client = CreateAuthorizedClient();

                int pageSize = 10;
                string endpoint = User.IsInRole("Admin")
                    ? $"talepforms/paged?page={page}&pageSize={pageSize}"
                    : $"talepforms/mine-paged?page={page}&pageSize={pageSize}";

                var response = await client.GetFromJsonAsync<PagedTalepFormResponse>(endpoint);

                if (response == null)
                    throw new Exception("API boş yanıt döndürdü.");

                ViewBag.CurrentPage = response.Page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)response.TotalCount / response.PageSize);
                ViewBag.TotalCount = response.TotalCount;
                ViewBag.PageSize = response.PageSize;

                return View(response.Data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Talep listesi alınamadı: " + ex.Message;
                return View(new List<ResultTalepFormDto>());
            }
        }

        // ================================
        // CREATE GET
        // ================================
        [HttpGet]
        public async Task<IActionResult> CreateTalepForm()
        {
            await SetDropdownsAsync();
            return View(new CreateTalepFormDto());
        }

        // ================================
        // CREATE POST
        // ================================
        [HttpPost]
        public async Task<IActionResult> CreateTalepForm(CreateTalepFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                return View(model);
            }

            try
            {
                var client = CreateAuthorizedClient();

                var response = await client.PostAsJsonAsync("talepforms", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Talep başarıyla oluşturuldu.";
                    return RedirectToAction("Index");
                }

                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Talep kaydedilemedi: " + error;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "İstek gönderilirken hata oluştu: " + ex.Message;
            }

            await SetDropdownsAsync();
            return View(model);
        }

        // ================================
        // DELETE
        // ================================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = CreateAuthorizedClient();

                var response = await client.DeleteAsync($"talepforms/{id}");

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Talep başarıyla silindi.";
                else
                    TempData["Error"] = "Talep silinirken hata oluştu.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ================================
        // EXPORT EXCEL
        // ================================
        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var client = CreateAuthorizedClient();

                var response = await client.GetAsync("talepforms/export-excel");

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    TempData["ExcelError"] = "İndirilecek veri bulunamadı.";
                    return RedirectToAction("Index");
                }

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ExcelError"] = "Excel dosyası alınamadı.";
                    return RedirectToAction("Index");
                }

                var content = await response.Content.ReadAsByteArrayAsync();

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "TalepForms.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ExcelError"] = "Hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
