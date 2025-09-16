using DogusCay.WebUI.DTOs.PointDtos;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class PointController : Controller
    {
        private readonly HttpClient _client;

        public PointController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }

        // Kullanıcının rolüne göre noktaları sayfalı olarak listeler
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                string endpoint;
                int pageSize = 10;

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
                ViewBag.CurrentPage = response.Page;
                ViewBag.PageSize = response.PageSize;
                ViewBag.TotalCount = response.TotalCount;
                ViewBag.TotalPages = (int)Math.Ceiling((double)response.TotalCount / response.PageSize);

                // Sadece veri listesini View'e gönderiyoruz
                return View(response.Data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Nokta listesi alınamadı: " + ex.Message;
                return View(new List<ResultPointDto>());
            }
        }
    }
}
