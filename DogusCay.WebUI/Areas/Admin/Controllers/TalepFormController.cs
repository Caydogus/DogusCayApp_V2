using DogusCay.WebUI.DTOs.TalepFormDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.DistributorDtos;
using DogusCay.WebUI.DTOs.PointDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using DogusCay.WebUI.DTOs.PointGroupTypeDtos;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class TalepFormController : Controller
    {
        private readonly HttpClient _client;

        public TalepFormController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("EduClient");
        }

        private async Task SetDropdownsAsync()
        {
            var kanallar = await _client.GetFromJsonAsync<List<KanalDropdownDto>>("kanals/dropdown");
            var urunler = await _client.GetFromJsonAsync<List<ProductDropdownDto>>("products/dropdown");

            ViewBag.Kanallar = new SelectList(kanallar, "KanalId", "KanalName");
            ViewBag.Products = new SelectList(urunler, "ProductId", "ProductName");

            // Diğer dropdown'lar başlangıçta boş olacak, JavaScript ile doldurulacak
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
            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                return View(createTalepFormDto);
            }

            var response = await _client.PostAsJsonAsync("talepforms", createTalepFormDto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            TempData["error"] = "Talep kaydedilemedi.";
            await SetDropdownsAsync();
            return View(createTalepFormDto);
        }
        
       
    }
}
