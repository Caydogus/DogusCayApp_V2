using DogusCay.WebUI.DTOs.PointDtos;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class PointController : Controller
    {
        private readonly HttpClient _client;

        public PointController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient"); // API base URL'si buraya ayarlanmış olmalı
        }

        // Kullanıcının rolüne göre noktaları sayfalı olarak listeler
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                string endpoint;
                int pageSize = 10; // Sayfa boyutunu burada belirleyebiliriz.

                // Kullanıcının rolüne göre API endpoint'ini belirle
                if (User.IsInRole("Admin"))
                {
                    endpoint = $"points/paged?page={page}&pageSize={pageSize}"; // Tüm noktalar (Admin için)
                }
                else // Bölge Müdürü veya diğer yetkili roller için
                {
                    endpoint = $"points/mine-paged?page={page}&pageSize={pageSize}"; // Sadece kendi noktaları
                }

                // API'den sayfalı veriyi çek
                var response = await _client.GetFromJsonAsync<PagedResult>(endpoint);

                // Sayfalama bilgilerini ViewBag'e ata
                // Bu değerler View'de sayfalama linklerini oluşturmak için kullanılacak
                ViewBag.CurrentPage = response.Page;
                ViewBag.PageSize = response.PageSize; // Burası önemli, View'de kullanılacak
                ViewBag.TotalCount = response.TotalCount;
                ViewBag.TotalPages = (int)Math.Ceiling((double)response.TotalCount / response.PageSize);


                // Sadece veri listesini View'e gönderiyoruz
                return View(response.Data);
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını ViewBag'e atıp boş bir liste gönder
                ViewBag.Error = "Nokta listesi alınamadı: " + ex.Message;
                return View(new List<ResultPointDto>()); // Boş bir liste ile View'i döndür
            }
        }
    }
}

