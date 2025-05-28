
using DogusCay.WebUI.DTOs.CategoryDtos;
using DogusCay.WebUI.DTOs.DistributorDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using DogusCay.WebUI.DTOs.TalepDtos;
using DogusCay.WebUI.Mappers;
using DogusCay.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TalepFormController : Controller
    {
        private readonly HttpClient _client;

        public TalepFormController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("EduClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _client.GetFromJsonAsync<List<ResultTalepFormDto>>("talepforms");
            return View(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _client.GetFromJsonAsync<ResultTalepFormDto>($"talepforms/{id}");
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _client.PostAsync($"talepforms/approve/{id}", null);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            await _client.PostAsync($"talepforms/reject/{id}", null);
            return RedirectToAction("Index");
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


        #region
        [HttpPost]
        public async Task<IActionResult> CreateTalepForm(TalepFormViewModel model, string submitType)
        {
            Console.WriteLine($"🔷 Formdan gelen submitType: {submitType}");
            if (model.Items == null || model.Items.Count == 0)
                model.Items = new List<TalepFormItemViewModel> { new TalepFormItemViewModel() };

            // 🔁 Dropdownlar (her zaman yeniden yüklenmeli)
            var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
            model.Kanallar = kanalList.Select(k => new SelectListItem
            {
                Value = k.KanalId.ToString(),
                Text = k.KanalName
            }).ToList();

            if (model.KanalId == 4)
            {
                var distributorList = await _client.GetFromJsonAsync<List<ResultDistributorDto>>($"distributors/by-kanal/{model.KanalId}");
                model.Distributors = distributorList.Select(d => new SelectListItem
                {
                    Value = d.DistributorId.ToString(),
                    Text = d.DistributorName
                }).ToList();

                if (model.DistributorId > 0)
                {
                    var pointGroupList = await _client.GetFromJsonAsync<List<ResultPointGroupTypeDto>>($"pointgrouptypes/by-distributor/{model.DistributorId}");
                    model.PointGruplar = pointGroupList?.Select(pg => new SelectListItem
                    {
                        Value = pg.PointGroupTypeId.ToString(),
                        Text = pg.PointGroupTypeName
                    }).ToList() ?? new();
                }

                if (model.DistributorId > 0 && model.PointGroupTypeId > 0)
                {
                    var pointList = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-group/{model.PointGroupTypeId}/distributor/{model.DistributorId}");
                    model.Noktalar = pointList.Select(p => new SelectListItem
                    {
                        Value = p.PointId.ToString(),
                        Text = p.PointName
                    }).ToList();
                }
            }
            else if (model.KanalId == 5 || model.KanalId == 6)
            {
                var points = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-kanal/{model.KanalId}");
                model.Noktalar = points.Select(p => new SelectListItem
                {
                    Value = p.PointId.ToString(),
                    Text = p.PointName
                }).ToList();
            }

            // 🧠 Ürün işlemleri (her ürün için zincir ve hesaplama)
            for (int i = 0; i < model.Items.Count; i++)
            {
                var item = model.Items[i];

                if (item.PreviousAnaKategoriId.HasValue && item.PreviousAnaKategoriId != item.AnaKategoriId)
                {
                    item.KategoriId = null;
                    item.AltKategoriId = null;
                    item.ProductId = 0;
                    item.Price = 0;
                    item.Total = 0;
                    item.Kategoriler = new();
                    item.AltKategoriler = new();
                    item.Urunler = new();
                }

                if (item.PreviousKategoryId.HasValue && item.PreviousKategoryId != item.KategoriId)
                {
                    item.AltKategoriId = null;
                    item.ProductId = 0;
                    item.Price = 0;
                    item.Total = 0;
                    item.Quantity = 0;
                    item.AltKategoriler = new();
                    item.Urunler = new();
                }

                if (item.PreviousAltKategoriId.HasValue && item.PreviousAltKategoriId != item.AltKategoriId)
                {
                    item.ProductId = 0;
                    item.Price = 0;
                    item.Quantity = 0;
                    item.Total = 0;
                    item.Urunler = new();
                }

                item.PreviousAnaKategoriId = item.AnaKategoriId;
                item.PreviousKategoryId = item.KategoriId;
                item.PreviousAltKategoriId = item.AltKategoriId;

                var anaKategoriler = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories/GetActiveCategories");
                item.AnaKategoriler = anaKategoriler
                    .Where(x => x.ParentCategoryId == null)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                if (item.AnaKategoriId.HasValue)
                {
                    var kategoriler = await _client.GetFromJsonAsync<List<ResultCategoryDto>>($"categories/{item.AnaKategoriId}/children");
                    item.Kategoriler = kategoriler.Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();
                }

                if (item.KategoriId.HasValue)
                {
                    var altKategoriler = await _client.GetFromJsonAsync<List<ResultCategoryDto>>($"categories/{item.KategoriId}/children");
                    item.AltKategoriler = altKategoriler.Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                    item.ShowAltKategori = item.AltKategoriler.Any();
                }

                int aktifKategoriId = item.AltKategoriId ?? item.KategoriId ?? 0;
                if (aktifKategoriId > 0)
                {
                    var urunler = await _client.GetFromJsonAsync<List<ResultProductDto>>($"products/GetProductsByCategoryId/{aktifKategoriId}");
                    item.Urunler = urunler.Select(p => new SelectListItem
                    {
                        Value = p.ProductId.ToString(),
                        Text = p.ProductName
                    }).ToList();
                }

                if (item.ProductId > 0)
                {
                    if (item.ProductId <= 0)
                    {
                        Console.WriteLine($"❌ Ürün seçilmemiş veya postback sırasında sıfırlanmış! [Index: {i}]");
                        continue; // Hatalıysa bu item'ı atla
                    }

                    var product = await _client.GetFromJsonAsync<ResultProductDto>($"products/details/{item.ProductId}");
                    if (product is not null)
                    {
                        item.Price = product.Price;
                        item.ErpCode = product.ErpCode;
                        item.ApproximateWeightKg = product.ApproximateWeightKg;
                        item.KoliIciAdet = product.KoliIciAdet;
                        if (item.Quantity == 0)
                            item.Quantity = 1;
                    }
                }

                decimal toplam = item.Quantity * item.Price;

                // İskonto uygulamaları
                if ( item.Iskonto1 > 0)
                    toplam *= (100 - item.Iskonto1) / 100m;

                if (item.Iskonto2 > 0)
                    toplam *= (100 - item.Iskonto2) / 100m;

                if ( item.Iskonto3 > 0)
                    toplam *= (100 - item.Iskonto3) / 100m;

                if (item.UseIskonto4 && item.Iskonto4 > 0)
                    toplam *= (100 - item.Iskonto4) / 100m;

                item.Total = toplam;
                item.KoliToplamAgirligiKg = item.Quantity * item.ApproximateWeightKg;
                item.KoliIciToplamAdet = item.KoliIciAdet * item.Quantity;

                // Liste fiyat: koli içi birim fiyat
                item.ListeFiyat = (item.KoliIciAdet > 0) ? item.Price / item.KoliIciAdet : 0;

                // Normalde iskontolu adet başı fiyat
                decimal calculatedSonAdetFiyati = (item.KoliIciToplamAdet > 0)
                    ? Math.Round(item.Total / item.KoliIciToplamAdet, 2)
                    : 0;

                // Eğer AdetFarkDonusuTL girildiyse, onunla yeniden hesapla
                if (item.AdetFarkDonusuTL > 0)
                {
                    item.SonAdetFiyati = Math.Round(calculatedSonAdetFiyati - item.AdetFarkDonusuTL, 2);
                }
                else
                {
                    item.SonAdetFiyati = calculatedSonAdetFiyati;
                }

               
            }

            // ✳️ Eğer kullanıcı Kaydet'e bastıysa → DTO'ya çevir ve gönder
            if (submitType == "save")
            {
                var dto = model.ToCreateDto(); // Extension method

                Console.WriteLine($"🟡 DTO'daki ürün sayısı: {dto.Items.Count}");
                foreach (var item in dto.Items)
                {
                    Console.WriteLine($"🟢 ProductId: {item.ProductId}, Quantity: {item.Quantity}, Price: {item.Price}");
                }
                await _client.PostAsJsonAsync("talepforms", dto);
                return RedirectToAction("Index");
            }
            {
                // sadece postback zinciri → dropdown/ürün güncelle
                ModelState.Clear();
            }
            return View(model);
        }
        #endregion
        #region
        //[HttpPost]
        //public async Task<IActionResult> CreateTalepForm(TalepFormViewModel model)
        //{
        //    ModelState.Clear();

        //    if (model.Items == null || model.Items.Count == 0)
        //        model.Items = new List<TalepFormItemViewModel> { new TalepFormItemViewModel() };

        //    // Kanal dropdown
        //    var kanalList = await _client.GetFromJsonAsync<List<ResultKanalDto>>("kanals");
        //    model.Kanallar = kanalList.Select(k => new SelectListItem
        //    {
        //        Value = k.KanalId.ToString(),
        //        Text = k.KanalName
        //    }).ToList();

        //    // Nokta zinciri
        //    if (model.KanalId == 4)
        //    {
        //        var distributorList = await _client.GetFromJsonAsync<List<ResultDistributorDto>>($"distributors/by-kanal/{model.KanalId}");
        //        model.Distributors = distributorList.Select(d => new SelectListItem
        //        {
        //            Value = d.DistributorId.ToString(),
        //            Text = d.DistributorName.Trim()
        //        }).ToList();

        //        if (model.DistributorId > 0)
        //        {
        //            var pointGroupList = await _client.GetFromJsonAsync<List<ResultPointGroupTypeDto>>($"pointgrouptypes/by-distributor/{model.DistributorId}");
        //            model.PointGruplar = pointGroupList?.Select(pg => new SelectListItem
        //            {
        //                Value = pg.PointGroupTypeId.ToString(),
        //                Text = pg.PointGroupTypeName
        //            }).ToList() ?? new();
        //        }

        //        if (model.DistributorId > 0 && model.PointGroupTypeId > 0)
        //        {
        //            var pointList = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-group/{model.PointGroupTypeId}/distributor/{model.DistributorId}");
        //            model.Noktalar = pointList.Select(p => new SelectListItem
        //            {
        //                Value = p.PointId.ToString(),
        //                Text = p.PointName
        //            }).ToList();
        //        }
        //    }
        //    else if (model.KanalId == 5 || model.KanalId == 6)
        //    {
        //        var points = await _client.GetFromJsonAsync<List<ResultPointDto>>($"points/by-kanal/{model.KanalId}");
        //        model.Noktalar = points.Select(p => new SelectListItem
        //        {
        //            Value = p.PointId.ToString(),
        //            Text = p.PointName
        //        }).ToList();
        //    }



        //    // Ürün ve kategori işlemleri
        //    for (int i = 0; i < model.Items.Count; i++)
        //    {
        //        var item = model.Items[i];

        //        // 🔁 Zincir temizleme kontrolleri (değişiklik varsa aşağı zinciri temizle)

        //        // 1️⃣ Ana kategori değiştiyse → kategori, alt kategori, ürün, fiyat, total temizle
        //        if (item.PreviousAnaKategoriId.HasValue && item.PreviousAnaKategoriId != item.AnaKategoriId)
        //        {
        //            item.KategoriId = null;
        //            item.AltKategoriId = null;
        //            item.ProductId = 0;
        //            item.Price = 0;
        //            item.Total = 0;
        //            item.Kategoriler = new();
        //            item.AltKategoriler = new();
        //            item.Urunler = new();
        //        }

        //        // 2️⃣ Kategori değiştiyse → alt kategori, ürün, fiyat, total temizle
        //        if (item.PreviousKategoryId.HasValue && item.PreviousKategoryId != item.KategoriId)
        //        {
        //            item.AltKategoriId = null;
        //            item.ProductId = 0;
        //            item.Price = 0;
        //            item.Total = 0;
        //            item.Quantity = 0;
        //            item.AltKategoriler = new();
        //            item.Urunler = new();
        //        }

        //        // 3️⃣ Alt kategori değiştiyse → ürün, fiyat, total temizle
        //        if (item.PreviousAltKategoriId.HasValue && item.PreviousAltKategoriId != item.AltKategoriId)
        //        {
        //            item.ProductId = 0;
        //            item.Price = 0;
        //            item.Quantity = 0;
        //            item.Total = 0;
        //            item.Urunler = new();
        //        }

        //        // 🔐 Mevcut değerleri sakla (bir sonraki post için)
        //        item.PreviousAnaKategoriId = item.AnaKategoriId;
        //        item.PreviousKategoryId = item.KategoriId;
        //        item.PreviousAltKategoriId = item.AltKategoriId;

        //        // 🟩 Ana kategoriler
        //        var anaKategoriler = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories/GetActiveCategories");
        //        item.AnaKategoriler = anaKategoriler
        //            .Where(x => x.ParentCategoryId == null)
        //            .Select(c => new SelectListItem
        //            {
        //                Value = c.CategoryId.ToString(),
        //                Text = c.CategoryName
        //            }).ToList();

        //        // 🟩 Kategoriler
        //        if (item.AnaKategoriId.HasValue)
        //        {
        //            var kategoriler = await _client.GetFromJsonAsync<List<ResultCategoryDto>>($"categories/{item.AnaKategoriId}/children");
        //            item.Kategoriler = kategoriler.Select(c => new SelectListItem
        //            {
        //                Value = c.CategoryId.ToString(),
        //                Text = c.CategoryName
        //            }).ToList();
        //        }

        //        // 🟩 Alt kategoriler
        //        if (item.KategoriId.HasValue)
        //        {
        //            var altKategoriler = await _client.GetFromJsonAsync<List<ResultCategoryDto>>($"categories/{item.KategoriId}/children");
        //            item.AltKategoriler = altKategoriler.Select(c => new SelectListItem
        //            {
        //                Value = c.CategoryId.ToString(),
        //                Text = c.CategoryName
        //            }).ToList();

        //            item.ShowAltKategori = item.AltKategoriler.Any();
        //        }

        //        // 🟩 Ürünler (aktif kategoriye göre)
        //        int aktifKategoriId = item.AltKategoriId ?? item.KategoriId ?? 0;
        //        if (aktifKategoriId > 0)
        //        {
        //            var urunler = await _client.GetFromJsonAsync<List<ResultProductDto>>($"products/GetProductsByCategoryId/{aktifKategoriId}");
        //            item.Urunler = urunler.Select(p => new SelectListItem
        //            {
        //                Value = p.ProductId.ToString(),
        //                Text = p.ProductName
        //            }).ToList();
        //        }

        //        // 🟩 Ürün detay bilgisi
        //        if (item.ProductId > 0)
        //        {
        //            var product = await _client.GetFromJsonAsync<ResultProductDto>($"products/details/{item.ProductId}");
        //            if (product is not null)
        //            {
        //                item.Price = product.Price;
        //                item.ErpCode = product.ErpCode;
        //                item.ApproximateWeightKg = product.ApproximateWeightKg;
        //                item.KoliIciAdet=product.KoliIciAdet;
        //                if (item.Quantity == 0)
        //                    item.Quantity = 1;
        //            }
        //        }

        //        // 🟩 Zincirli İskonto Uygulaması
        //        decimal toplam = item.Quantity * item.Price;


        //        if (item.UseIskonto1 && item.Iskonto1 > 0)
        //            toplam *= (100 - item.Iskonto1) / 100m;

        //        if (item.UseIskonto2 && item.Iskonto2 > 0)
        //            toplam *= (100 - item.Iskonto2) / 100m;

        //        if (item.UseIskonto3 && item.Iskonto3 > 0)
        //            toplam *= (100 - item.Iskonto3) / 100m;

        //        if (item.UseIskonto4 && item.Iskonto4 > 0)
        //            toplam *= (100 - item.Iskonto4) / 100m;

        //        // 💰 Final Total
        //        item.Total = toplam;
        //        item.KoliToplamAgirligiKg = item.Quantity * item.ApproximateWeightKg;
        //        item.KoliIciToplamAdet = item.KoliIciAdet * item.Quantity; // Koli içi adet, toplam koli sayısını etkiler
        //    }        
        //    return View(model);

        //}
        #endregion
    }
}
