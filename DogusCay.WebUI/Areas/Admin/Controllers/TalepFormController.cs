using DogusCay.WebUI.DTOs.CategoryDtos;
using DogusCay.WebUI.DTOs.DistributorDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using DogusCay.WebUI.DTOs.TalepFormDtos;
using DogusCay.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpGet]
        public async Task<IActionResult> CreateTalepForm()
        {
            var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");

            var model = new TalepFormViewModel
            {
                Kanallar = kanalList.Select(k => new SelectListItem
                {
                    Value = k.KanalId.ToString(),
                    Text = k.KanalName
                }).ToList()
            };

            model.Items.Add(new TalepFormItemViewModel());
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTalepForm(TalepFormViewModel model)
        {
            if (model.Items == null || model.Items.Count == 0)
                model.Items = new List<TalepFormItemViewModel> { new TalepFormItemViewModel() };

            var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            model.Kanallar = kanalList.Select(k => new SelectListItem
            {
                Value = k.KanalId.ToString(),
                Text = k.KanalName
            }).ToList();

            // 🎯 KanalId üzerinden kontrol
            if (model.KanalId == 4) // DIST Kanalı ID'si
            {
                var distributorList = await _client.GetFromJsonAsync<List<ResultDistributorDto>>($"distributors/by-kanal/{model.KanalId}");
                model.Distributors = distributorList.Select(d => new SelectListItem
                {
                    Value = d.DistributorId.ToString(),
                    Text = d.DistributorName.Trim()
                }).ToList();

                if (model.DistributorId > 0)
                {
                    var pointGroupList = await _client.GetFromJsonAsync<List<ResultPointGroupTypeDto>>($"pointgrouptypes/by-distributor/{model.DistributorId}");
                    model.PointGruplar = pointGroupList?.Select(pg => new SelectListItem
                    {
                        Value = pg.PointGroupTypeId.ToString(),
                        Text = pg.PointGroupTypeName
                    }).ToList() ?? new List<SelectListItem>(); // null kontrolü
                }

                if (model.DistributorId > 0 && model.PointGroupTypeId > 0)
                {
                    var pointList = await _client.GetFromJsonAsync<List<ResultPointDto>>(
                        $"points/by-group/{model.PointGroupTypeId}/distributor/{model.DistributorId}");
                    model.Noktalar = pointList.Select(p => new SelectListItem
                    {
                        Value = p.PointId.ToString(),
                        Text = p.PointName
                    }).ToList();
                }

                return View(model);
            }

            // LC veya NA (örnek: 5 ve 6)
            if (model.KanalId == 5 || model.KanalId == 6)
            {
                var points = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-kanal/{model.KanalId}");
                model.Noktalar = points.Select(p => new SelectListItem
                {
                    Value = p.PointId.ToString(),
                    Text = p.PointName
                }).ToList();

                return View(model);
            }

            return View(model);
        }


    }
}
