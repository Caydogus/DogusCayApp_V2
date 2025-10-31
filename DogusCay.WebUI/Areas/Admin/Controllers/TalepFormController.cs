using DogusCay.WebUI.DTOs.TalepDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class TalepFormController : Controller
    {
        private readonly HttpClient _client;

        public TalepFormController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("EduClient");
        }

        private async Task SetDropdownsAsync()
        {
            var jwtToken = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(jwtToken))
            {
                Console.WriteLine("Uyarı: Session'dan JWT token alınamadı!");
                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
                TempData["Error"] = "Dropdown verileri yüklenirken bir hata oluştu: Oturum süresi dolmuş veya token bulunamadı.";
                return;
            }

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

            try
            {
                var kanallar = await _client.GetFromJsonAsync<List<KanalDropdownDto>>("kanals/dropdown");
                ViewBag.Kanallar = new SelectList(kanallar, "KanalId", "KanalName");

                var urunler = await _client.GetFromJsonAsync<List<ProductDropdownDto>>("products/dropdown");
                ViewBag.Products = new SelectList(urunler, "ProductId", "ProductName");
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"❌ HTTP Request Error (SetDropdownsAsync): {httpEx.Message}");
                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
                TempData["Error"] = $"Dropdown verileri yüklenirken bir HTTP hatası oluştu: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching dropdown data (SetDropdownsAsync): {ex.Message}");
                ViewBag.Kanallar = new SelectList(new List<KanalDropdownDto>(), "KanalId", "KanalName");
                ViewBag.Products = new SelectList(new List<ProductDropdownDto>(), "ProductId", "ProductName");
                TempData["Error"] = "Dropdown verileri yüklenirken beklenmeyen bir hata oluştu: " + ex.Message;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                string endpoint;
                int pageSize = 10;

                if (User.IsInRole("Admin"))
                    endpoint = $"talepforms/paged?page={page}&pageSize={pageSize}";
                else
                    endpoint = $"talepforms/mine-paged?page={page}&pageSize={pageSize}";

                var response = await _client.GetFromJsonAsync<PagedTalepFormResponse>(endpoint);

                ViewBag.CurrentPage = response.Page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)response.TotalCount / response.PageSize);
                ViewBag.TotalCount = response.TotalCount;
                ViewBag.PageSize = response.PageSize;

                return View(response.Data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Talep listesi alınamadı: " + ex.Message;
                return View(new List<ResultTalepFormDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateTalepForm()
        {
            await SetDropdownsAsync();
            return View(new CreateTalepFormDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTalepForm(CreateTalepFormDto createTalepFormDto)
        {
            Console.WriteLine("METODA GELDİ");

            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                return View(createTalepFormDto);
            }

            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(createTalepFormDto);
                Console.WriteLine("Gönderilen JSON:");
                Console.WriteLine(json);

                var response = await _client.PostAsJsonAsync("talepforms", createTalepFormDto);
                var responseText = await response.Content.ReadAsStringAsync();

                Console.WriteLine("POST Status: " + response.StatusCode);
                Console.WriteLine("API Yanıtı: " + responseText);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                TempData["error"] = "Talep kaydedilemedi.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ HATA: " + ex.Message);
                TempData["error"] = "İstek gönderilirken hata oluştu: " + ex.Message;
            }

            await SetDropdownsAsync();
            return View(createTalepFormDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"talepforms/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Talep başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = "Talep silinirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var response = await _client.GetAsync("talepforms/export-excel");

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
            var fileName = "TalepForms.xlsx";

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }


    }
}
