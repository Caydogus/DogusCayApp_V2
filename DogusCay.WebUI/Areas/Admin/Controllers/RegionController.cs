using DogusCay.WebUI.DTOs.RegionDtos;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class RegionController : Controller
    {
        private readonly HttpClient _client;

        public RegionController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("EduClient");
        }

        public async Task<IActionResult> Index()
        {
            var regions = await _client.GetFromJsonAsync<List<ResultRegionDto>>("Regions/WithManager");
            return View(regions);
        }
    }
}
