using DogusCay.WebUI.DTOs.CategoryDtos;
using DogusCay.WebUI.DTOs.DistributorDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
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
            // 🔁 Kanal dropdown'larını yükle
            var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            model.Kanallar = kanalList.Select(k => new SelectListItem
            {
                Value = k.KanalId.ToString(),
                Text = k.KanalName
            }).ToList();

            // 🔍 Kanal adına göre davranış
            var kanalAdi = kanalList.FirstOrDefault(k => k.KanalId == model.KanalId)?.KanalName;

            if (kanalAdi == "DIST")
            {
                // 🔹 Distributor
                var distributorList = await _client.GetFromJsonAsync<List<ResultDistributorDto>>($"distributors/by-kanal/{model.KanalId}");
                model.Distributors = distributorList.Select(d => new SelectListItem
                {
                    Value = d.DistributorId.ToString(),
                    Text = d.DistributorName
                }).ToList();

                // 🔹 Point Group (distributor’a bağlı)
                if (model.DistributorId > 0)
                {
                    var pointGroupList = await _client.GetFromJsonAsync<List<ResultPointGroupTypeDto>>($"pointgroups/by-distributor/{model.DistributorId}");
                    model.PointGruplar = pointGroupList.Select(pg => new SelectListItem
                    {
                        Value = pg.PointGroupTypeId.ToString(),
                        Text = pg.PointGroupTypeName
                    }).ToList();
                }

                // 🔹 Noktalar (point group’a bağlı)
                if (model.PointGroupId > 0)
                {
                    var pointList = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-pointgroup/{model.PointGroupId}");
                    model.Noktalar = pointList.Select(p => new SelectListItem
                    {
                        Value = p.PointId.ToString(),
                        Text = p.PointName
                    }).ToList();
                }
            }
            else if (kanalAdi == "LC" || kanalAdi == "NA")
            {
                // 🔹 Noktaları direkt yükle
                var pointList = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-kanal/{model.KanalId}");
                model.Noktalar = pointList.Select(p => new SelectListItem
                {
                    Value = p.PointId.ToString(),
                    Text = p.PointName
                }).ToList();
            }

            // 🔽 Ürün zinciri için: sadece ilk item (isteğe bağlı olarak döngüye alınabilir)
            var item = model.Items[0];
            var allCategories = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories");

            item.AnaKategoriler = allCategories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            if (item.AnaKategoriId is > 0)
            {
                item.Kategoriler = allCategories
                    .Where(c => c.ParentCategoryId == item.AnaKategoriId)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();
            }

            if (item.KategoriId is > 0)
            {
                item.AltKategoriler = allCategories
                    .Where(c => c.ParentCategoryId == item.KategoriId)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();
            }

            if (item.AltKategoriId is > 0)
            {
                var urunler = await _client.GetFromJsonAsync<List<ResultProductDto>>($"products/by-subcategory/{item.AltKategoriId}");
                item.Urunler = urunler.Select(p => new SelectListItem
                {
                    Value = p.ProductId.ToString(),
                    Text = p.ProductName
                }).ToList();
            }

            if (item.ProductId is > 0)
            {
                var product = await _client.GetFromJsonAsync<ResultProductDto>($"products/details/{item.ProductId}");
                item.Price = product.Price;
            }

            // ✅ Doğrulama
            if (!ModelState.IsValid || model.PointId == 0 || item.ProductId == 0)
            {
                return View(model);
            }

            // 📤 API'ye gönder
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
