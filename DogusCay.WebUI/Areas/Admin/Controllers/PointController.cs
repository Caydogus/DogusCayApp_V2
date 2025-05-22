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
            _client = clientFactory.CreateClient("EduClient");
        }

        public async  Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultPointDto>>("points");
            return View(values);
        }
    }
}
