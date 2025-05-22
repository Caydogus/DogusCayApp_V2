using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class PointGroupTypeController : Controller
    {
        private readonly HttpClient _client;

        public PointGroupTypeController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }

        //nokta gruplarını kanallarıyla beraber getir
        public async Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultPointGroupTypeDto>>("pointgrouptypes");
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePointGroupType()
        {

            var response = await _client.GetFromJsonAsync<List<ResultPointGroupTypeDto>>("pointgrouptypes");
            ViewBag.PointGroupTypes = new SelectList(response, "PointGroupTypeId", "PointGroupTypeName");


            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreatePointGroupType(CreatePointGroupTypeDto createPointGroupTypeDto)
        {
            return View(createPointGroupTypeDto);
        }

    }
}