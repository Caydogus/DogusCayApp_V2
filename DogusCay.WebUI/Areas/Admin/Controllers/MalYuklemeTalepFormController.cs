using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.MalYuklemeTalepFormDtos;
using Newtonsoft.Json;
using DogusCay.WebUI.DTOs.ProductDtos;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class MalYuklemeTalepFormController : Controller
    {
        private readonly HttpClient _client;

        public MalYuklemeTalepFormController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> CreateMalYuklemeTalepForm()
        {
            await SetDropdownsAsync();
            return View(new CreateMalYuklemeTalepFormDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMalYuklemeTalepForm(CreateMalYuklemeTalepFormDto createMalYuklemeTalepFormDto)
        {
            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                return View(createMalYuklemeTalepFormDto);
            }
            try
            {
                var response = await _client.PostAsJsonAsync("malyuklemetalepforms", createMalYuklemeTalepFormDto);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Mal Yükleme talebi başarıyla oluşturuldu.";
                    return RedirectToAction("Index", "TalepForm", new { area = "Admin" });
                }
                else
                {
                    TempData["Error"] = $"Talep kaydedilemedi: {responseText}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ HTTP İstek Hatası: {ex.Message}");
                TempData["Error"] = "İstek gönderilirken bir hata oluştu: " + ex.Message;
            }

            await SetDropdownsAsync();
            return View(createMalYuklemeTalepFormDto);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 7)
        {
            string endpoint;
            bool isAdmin = User.IsInRole("Admin");

            if (isAdmin)
            {
                endpoint = $"malyuklemetalepforms/paged?page={page}&pageSize={pageSize}";
                ViewBag.Title = "Tüm Mal Yükleme Talepleri";
            }
            else
            {
                endpoint = $"malyuklemetalepforms/mine-paged?page={page}&pageSize={pageSize}";
                ViewBag.Title = "Mal Yükleme Taleplerim";
            }

            try
            {
                var token = HttpContext.Session.GetString("JwtToken");
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<PagedMalYuklemeTalepFormResponse>(jsonResponse);

                ViewBag.CurrentPage = pagedResult.Page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / pagedResult.PageSize);
                ViewBag.PageSize = pagedResult.PageSize;

                return View(pagedResult.Data);
            }
            catch (HttpRequestException ex)
            {
                TempData["Error"] = "Talepler getirilirken bir hata oluştu: " + ex.Message;
                return View(new List<ResultMalYuklemeTalepFormDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Beklenmeyen bir hata oluştu: " + ex.Message;
                return View(new List<ResultMalYuklemeTalepFormDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _client.GetAsync($"malyuklemetalepforms/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var formDetail = JsonConvert.DeserializeObject<ResultMalYuklemeTalepFormDto>(jsonResponse);
                    return View(formDetail);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    TempData["Error"] = $"ID'si {id} olan mal yükleme talebi bulunamadı.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Mal yükleme talebi detayları getirilirken hata oluştu: {response.StatusCode} - {errorContent}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Mal yükleme talebi detayları getirilirken beklenmeyen bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var response = await _client.PostAsync($"malyuklemetalepforms/approve/{id}", null);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var response = await _client.PostAsync($"malyuklemetalepforms/reject/{id}", null);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.";
                    return RedirectToAction("Index");
                }

                var request = new HttpRequestMessage(HttpMethod.Delete, $"malyuklemetalepforms/{id}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Talep başarıyla silindi.";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = "Silme başarısız: " + error;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel() //MalYuklemeTalepForms için Excel'e Aktar
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.";
                return RedirectToAction("Index");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "malyuklemetalepforms/export-excel");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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
            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "MalYuklemeTalepForms.xlsx");
        }

    }
}
