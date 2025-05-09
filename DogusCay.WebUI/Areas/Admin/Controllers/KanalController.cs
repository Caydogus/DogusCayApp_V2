using DogusCay.WebUI.DTOs.KanalDtos;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class KanalController : Controller
    {
        private readonly HttpClient _client;

        public KanalController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }


        public async Task<IActionResult> Index()
        {

            var values = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            return View(values);
        }
        public async Task<IActionResult> DeleteKanal(int id)
        {
            await _client.DeleteAsync($"kanals/{id}");
            return RedirectToAction(nameof(Index));
        }


        public IActionResult CreateKanal()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateKanal(CreateKanalDto createKanalDto)
        {
            await _client.PostAsJsonAsync("kanals", createKanalDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateKanal(int id)
        {
            var values = await _client.GetFromJsonAsync<UpdateKanalDto>($"kanals/{id}");
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateKanal(UpdateKanalDto updateKanalDto)
        {
            await _client.PutAsJsonAsync("kanals", updateKanalDto);
            return RedirectToAction(nameof(Index));
        }
    }
}

