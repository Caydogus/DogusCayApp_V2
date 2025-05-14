using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
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

        // Talep formlarını listeleme sayfası (API'den veri çeker)
        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<TalepFormListViewModel>>("talepforms/admin");
            return View(list);
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

            model.Items.Add(new TalepFormItemViewModel()); // ilk satır boş gelsin
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTalepForm(TalepFormViewModel model)
        {
            // Kanallar her zaman yüklensin (geri dönüşte lazım)
            var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            model.Kanallar = kanalList.Select(k => new SelectListItem
            {
                Value = k.KanalId.ToString(),
                Text = k.KanalName
            }).ToList();

            // Kanal seçilmişse → Nokta grubu getir
            if (model.KanalId > 0)
            {
                var pointGroupList = await _client.GetFromJsonAsync<List<ResultPointGroupDto>>($"pointgroups/by-kanal/{model.KanalId}");
                model.PointGruplar = pointGroupList.Select(pg => new SelectListItem
                {
                    Value = pg.PointGroupId.ToString(),
                    Text = pg.GroupName
                }).ToList();
            }

            // Nokta grubu seçilmişse → Noktaları getir
            if (model.PointGroupId > 0)
            {
                var pointList = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-pointgroup/{model.PointGroupId}");
                model.Noktalar = pointList.Select(p => new SelectListItem
                {
                    Value = p.PointId.ToString(),
                    Text = p.PointName
                }).ToList();
            }

            // Eğer form submit edildiyse ama geçerli değilse (dropdown seçimleri yapılmış ama eksik alan var)
            if (!ModelState.IsValid || model.PointId == 0)
            {
                return View(model);
            }

            // Başarılı ise API'ye gönder
            var response = await _client.PostAsJsonAsync("talepforms", model);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Talep başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Talep oluşturulamadı.";
            return View(model);
        }


    }
}
