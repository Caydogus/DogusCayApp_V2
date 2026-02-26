using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DogusCay.WebUI.DTOs.IhaleAnlasmaDtos;
using DogusCay.WebUI.DTOs.TalepDtos;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class IhaleAnlasmaSozlesmeController : Controller
    {
        private readonly HttpClient _client;

        public IhaleAnlasmaSozlesmeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("EduClient");
        }

        private void SetAuthHeader()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            _client.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SetAuthHeader();
            try
            {
                string endpoint;
                bool isAdmin = User.IsInRole("Admin");

                if (isAdmin)
                {
                    endpoint = "ihaleanlasmalar/all-list";
                    ViewBag.Title = "Tüm İhale Anlaşmaları";
                }
                else
                {
                    endpoint = "ihaleanlasmalar/my-list";
                    ViewBag.Title = "İhale Anlaşmalarım";
                }

                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<ResultIhaleAnlasmaDto>>(json);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Veriler yüklenirken hata oluştu: " + ex.Message;
                return View(new List<ResultIhaleAnlasmaDto>());
            }
        }

        // Sözleşme yükleme sayfası
        [HttpGet]   
        public IActionResult CreateSozlesme(string noktaKod)
        {
            var model = new CreateIhaleAnlasmaSozlesmeViewModel
            {
                NoktaKod = noktaKod
            };
            return View(model);
        }

        // Sözleşme yükleme POST
        [HttpPost]
        public async Task<IActionResult> CreateSozlesme(CreateIhaleAnlasmaSozlesmeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            SetAuthHeader();

            try
            {
                using var content = new MultipartFormDataContent();

                content.Add(new StringContent(model.NoktaKod ?? ""), "NoktaKod");

                if (model.IskontoOrani.HasValue)
                {
                    content.Add(
                        new StringContent(
                            model.IskontoOrani.Value.ToString(
                                System.Globalization.CultureInfo.InvariantCulture
                            )
                        ),
                        "IskontoOrani"
                    );
                }

                if (!string.IsNullOrWhiteSpace(model.Note))
                {
                    content.Add(new StringContent(model.Note), "Note");
                }

                if (model.Dosyalar != null)
                {
                    foreach (var file in model.Dosyalar)
                    {
                        var streamContent = new StreamContent(file.OpenReadStream());
                        streamContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                        content.Add(streamContent, "dosyalar", file.FileName);
                    }
                }

                var response = await _client.PostAsync("ihaleanlasmalar/sozlesme", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Sözleşme başarıyla yüklendi.";
                    return RedirectToAction("Index");
                }

                TempData["Error"] = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(model);
        }

        // Sözleşme detay
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            SetAuthHeader();
            try
            {
                var response = await _client.GetAsync($"ihaleanlasmalar/sozlesme/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<ResultIhaleAnlasmaSozlesmeDto>(json);
                    return View(detail);
                }

                TempData["Error"] = "Sözleşme detayı getirilemedi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Sözleşme düzenleme sayfası
        [HttpGet]
        public async Task<IActionResult> EditSozlesme(int id)
        {
            if (!User.IsInRole("BolgeMuduru"))
                return RedirectToAction("Index");

            SetAuthHeader();
            try
            {
                var response = await _client.GetAsync($"ihaleanlasmalar/sozlesme/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Sözleşme bulunamadı.";
                    return RedirectToAction("Index");
                }

                var json = await response.Content.ReadAsStringAsync();
                var detail = JsonConvert.DeserializeObject<ResultIhaleAnlasmaSozlesmeDto>(json);

                if (detail.TalepDurumu == TalepDurumu.Onaylandi)
                {
                    TempData["Error"] = "Onaylanmış sözleşmeler düzenlenemez.";
                    return RedirectToAction("Index");
                }

                var model = new EditIhaleAnlasmaSozlesmeViewModel
                {
                    IhaleAnlasmaSozlesmeId = detail.IhaleAnlasmaSozlesmeId,
                    NoktaKod = detail.NoktaKod,
                    NoktaAdi = detail.NoktaAdi,
                    IskontoOrani = detail.IskontoOrani,
                    Note = detail.Note,
                    MevcutDosyalar = detail.Dosyalar
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Sözleşme düzenleme POST
        [HttpPost]
        public async Task<IActionResult> EditSozlesme(EditIhaleAnlasmaSozlesmeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Mevcut dosyaları tekrar yükle
                SetAuthHeader();
                var resp = await _client.GetAsync($"ihaleanlasmalar/sozlesme/{model.IhaleAnlasmaSozlesmeId}");
                if (resp.IsSuccessStatusCode)
                {
                    var j = await resp.Content.ReadAsStringAsync();
                    var d = JsonConvert.DeserializeObject<ResultIhaleAnlasmaSozlesmeDto>(j);
                    model.MevcutDosyalar = d.Dosyalar;
                }
                return View(model);
            }

            SetAuthHeader();
            try
            {
                using var content = new MultipartFormDataContent();

                content.Add(new StringContent(
                    model.IskontoOrani.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                    "iskontoOrani");

                if (!string.IsNullOrWhiteSpace(model.Note))
                    content.Add(new StringContent(model.Note), "note");

                if (model.YeniDosyalar != null)
                {
                    foreach (var file in model.YeniDosyalar)
                    {
                        var streamContent = new StreamContent(file.OpenReadStream());
                        streamContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                        content.Add(streamContent, "yeniDosyalar", file.FileName);
                    }
                }

                var response = await _client.PutAsync(
                    $"ihaleanlasmalar/sozlesme/{model.IhaleAnlasmaSozlesmeId}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Sözleşme güncellendi.";
                    return RedirectToAction("Detail", new { id = model.IhaleAnlasmaSozlesmeId });
                }

                TempData["Error"] = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("EditSozlesme", new { id = model.IhaleAnlasmaSozlesmeId });
        }

        // Tekil dosya silme (AJAX)
        [HttpPost]
        public async Task<IActionResult> DeleteDosya(int dosyaId, int sozlesmeId)
        {
            SetAuthHeader();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"ihaleanlasmalar/dosya/{dosyaId}");
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Dosya silindi.";
            else
                TempData["Error"] = "Dosya silinemedi: " + await response.Content.ReadAsStringAsync();

            return RedirectToAction("EditSozlesme", new { id = sozlesmeId });
        }


        //// Admin onay
        //[HttpPost]
        //public async Task<IActionResult> Approve(int id)
        //{
        //    SetAuthHeader();
        //    await _client.PostAsync($"ihaleanlasmalar/sozlesme/approve/{id}", null);
        //    //return RedirectToAction("Index");
        //    return Ok();
        //}

        //// Admin red
        //[HttpPost]
        //public async Task<IActionResult> Reject(int id)
        //{
        //    SetAuthHeader();
        //    await _client.PostAsync($"ihaleanlasmalar/sozlesme/reject/{id}", null);
        //    //return RedirectToAction("Index");
        //    return Ok();
        //}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            SetAuthHeader();
            await _client.PostAsync($"ihaleanlasmalar/sozlesme/approve/{id}", null);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            TempData["Success"] = "Sözleşme onaylandı.";
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            SetAuthHeader();
            await _client.PostAsync($"ihaleanlasmalar/sozlesme/reject/{id}", null);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            TempData["Success"] = "Sözleşme reddedildi.";
            return RedirectToAction("Detail", new { id });
        }
        // Admin - Tüm noktalar
        [HttpGet]
        public async Task<IActionResult> TumNoktalar()
        {
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Index");

            SetAuthHeader();
            try
            {
                var response = await _client.GetAsync("ihaleanlasmalar/all-noktalar");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<ResultIhaleAnlasmaDto>>(json);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Veriler yüklenirken hata oluştu: " + ex.Message;
                return View(new List<ResultIhaleAnlasmaDto>());
            }
        }

        // Silme
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            SetAuthHeader();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"ihaleanlasmalar/sozlesme/{id}");
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Sözleşme silindi.";
                else
                    TempData["Error"] = "Silme başarısız: " + await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}