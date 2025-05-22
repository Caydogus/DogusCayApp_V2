using System.Security.Claims;
using DogusCay.WebUI.DTOs.DistributorDtos;
using DogusCay.WebUI.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class DistributorController : Controller
    {
        private readonly HttpClient _client;

        public DistributorController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }


        public async Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultDistributorDto>>("Distributors");
            return View(values);
        }

    }
}
