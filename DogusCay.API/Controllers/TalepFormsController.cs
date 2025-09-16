using AutoMapper;
using DogusCay.API.Helpers;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{

    [Authorize] // Yetkilendirme gerektiren controller
    [Route("api/talepforms")]
    [ApiController]
    public class TalepFormsController : ControllerBase
    {
        private readonly ITalepFormService _talepFormService;
        private readonly IMapper _mapper;
        private readonly MailHelper _mailHelper;

    
        public TalepFormsController(ITalepFormService talepFormService, IMapper mapper, MailHelper mailHelper)
        {
            _talepFormService = talepFormService;
            _mapper = mapper;
            _mailHelper = mailHelper;
        }

        // Kullanıcının ID'sini alır
        // Bu metot User nesnesi ClaimTypes.NameIdentifier'dan okuyacaktır.
        private int GetUserId()
        {
            var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        [HttpGet("mine-paged")]
        [Authorize(Roles = "BolgeMuduru")]
        public IActionResult GetOwnFormsPaged(int page = 1, int pageSize = 7)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı ID'si alınamadı.");

            var userForms = _talepFormService.TGetAllByUserId(userId).AsQueryable();

            var totalCount = userForms.Count();
            var pagedData = userForms
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = _mapper.Map<List<ResultTalepFormDto>>(pagedData);

            return Ok(new
            {
                Data = dtoList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpGet("paged")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllFormsPaged(int page = 1, int pageSize = 7)
        {
            var allForms = _talepFormService.TGetAllWithUser().AsQueryable();

            var totalCount = allForms.Count();
            var pagedData = allForms
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = _mapper.Map<List<ResultTalepFormDto>>(pagedData);

            return Ok(new
            {
                Data = dtoList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] CreateTalepFormDto dto)
        {
            int authenticatedUserId = GetUserId();
            if (authenticatedUserId == 0)
                return Unauthorized("Kullanıcı ID'si token'dan alınamadı.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 📌 Önce talep formunu kaydet
            var form = _talepFormService.CreateTalepFormWithCalculations(dto, authenticatedUserId);

            // DB’den tekrar çek, navigation property’leri dolu olsun
            form = _talepFormService.TGetByIdWithUserAndPoint(form.TalepFormId);

            var userName = form.AppUser != null
                ? $"{form.AppUser.FirstName} {form.AppUser.LastName}"
                : "Bilinmeyen Kullanıcı";

            var pointName = form.Point != null
                ? form.Point.PointName
                : "Bilinmeyen Müşteri";

            // 📧 Mail gönderimini API cevabına bağlamadık
            Task.Run(() =>
            {
                try
                {
                    var notify = _mailHelper.GetNotifyAddress();
                    _mailHelper.Send(notify,
                        "Yeni Insert Talep Oluşturuldu",
                        $"Yeni insert talep oluşturuldu.\n" +
                        $"Talep No: {form.TalepFormId}\n" +
                        $"Kullanıcı: {userName}\n" +
                        $"Müşteri (Nokta): {pointName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("📧 Mail gönderilemedi: " + ex.Message);
                    
                }
            });

            return Ok("Talep başarıyla oluşturuldu.");
        }


        // Belirli bir talep formunun detaylarını getirir.
        [HttpGet("{id}")]
        [Authorize] // Giriş yapmış herkes erişebilir
        public IActionResult GetDetails(int id)
        {
            var entity = _talepFormService.TGetDetailsForForm(id);
            if (entity == null) return NotFound("Talep bulunamadı.");
            var dto = _mapper.Map<ResultTalepFormDto>(entity);
            return Ok(dto);
        }
        // Talep formunu onaylar (Admin rolü için).
        [HttpPost("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            int adminId = GetUserId();
            if (adminId == 0)
                return Unauthorized("Admin ID'si token'dan alınamadı.");

            var form = _talepFormService.TGetByIdWithUserAndPoint(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            _talepFormService.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);

            // 📧 Mail gönderimini API'ye bağlamadık
            Task.Run(() =>
            {
                try
                {
                    var userEmail = form.AppUser?.Email;
                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        _mailHelper.Send(
                            userEmail,
                            "İnsert Talebiniz Onaylandı",
                            $"Merhaba {form.AppUser.FirstName} {form.AppUser.LastName},\n" +
                            $"{form.Point?.PointName ?? "Bilinmeyen Müşteri"} için açmış olduğunuz " +
                            $"{form.TalepFormId} numaralı talep onaylandı."
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("📧 Onay maili gönderilemedi: " + ex.Message);
                    
                }
            });

            return Ok("Talep onaylandı.");
        }




        // Talep formunu reddeder (Admin rolü için).
        [HttpPost("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            int adminId = GetUserId();
            if (adminId == 0)
                return Unauthorized("Admin ID'si token'dan alınamadı.");

            var form = _talepFormService.TGetByIdWithUserAndPoint(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            _talepFormService.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);

            // 📧 Mail gönderimi opsiyonel ve asenkron
            Task.Run(() =>
            {
                try
                {
                    var userEmail = form.AppUser?.Email;
                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        _mailHelper.Send(
                            userEmail,
                            "İnsert Talebiniz Reddedildi",
                            $"Merhaba {form.AppUser.FirstName} {form.AppUser.LastName},\n" +
                            $"{form.Point?.PointName ?? "Bilinmeyen Müşteri"} için açmış olduğunuz " +
                            $"{form.TalepFormId} numaralı talep reddedildi."
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("📧 Red maili gönderilemedi: " + ex.Message);
                    // TODO: ILogger/Serilog ile loglanabilir
                }
            });

            return Ok("Talep reddedildi.");
        }



        // Admin tüm talep formunu günceller.
        [HttpPut]
        [Authorize(Roles = "Admin")] // Sadece Admin rolündekiler erişebilir
        public IActionResult Update([FromBody] UpdateTalepFormDto dto)
        {
            var entity = _mapper.Map<TalepForm>(dto);
            // Güncelleme yapan Admin'in ID'sini de buraya ekleyebilirsiniz, eğer logluyorsanız
            _talepFormService.TUpdate(entity);
            return Ok("Talep güncellendi.");
        }

        // Kullanıcının kendi onaylanmamış formunu silebilir.
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var form = _talepFormService.TGetById(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            _talepFormService.TDelete(id);
            return Ok("Talep silindi.");
        }

        //Toplam form sayısını getirir.
        [HttpGet("count")]
        [Authorize] // Giriş yapmış herkes erişebilir
        public IActionResult GetFormCount()
        {
            var count = _talepFormService.TCount();
            return Ok(count);
        }

        // Bölge Müdürü kampanya sonrası kaç adet ürün satıldığını girer.
        [HttpPatch("update-donus/{id}")]
        [Authorize(Roles = "BolgeMuduru")] // Sadece Bölge Müdürü erişebilir
        public IActionResult UpdateKampanyaDonus(int id, [FromBody] int kampanyaDonusAdedi)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı ID'si alınamadı.");

            var form = _talepFormService.TGetById(id);
            if (form == null)
                return NotFound("Talep formu bulunamadı.");

            // Sadece kendi formunu ve onaylanmış olanı güncelleyebilsin
            if (form.AppUserId != userId)
                return Forbid("Bu forma erişim yetkiniz yok.");

            if (form.TalepDurumu != TalepDurumu.Onaylandi)
                return BadRequest("Sadece onaylanmış talepler için kampanya dönüşü girilebilir.");

            form.KampanyaDonusAdedi = kampanyaDonusAdedi;

            _talepFormService.TUpdate(form);

            return Ok("Kampanya dönüşü başarıyla kaydedildi.");
        }


        // Bölge Müdürü kampanyanın resmini girer.
        [HttpPost("upload-image/{id}")]
        [Authorize(Roles = "BolgeMuduru")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            var form = _talepFormService.TGetById(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            //Eski resmi sil
            if (!string.IsNullOrEmpty(form.KampanyaResimYolu))
            {
                var existingPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "DogusCay.WebUI", "wwwroot", form.KampanyaResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(existingPath))
                {
                    System.IO.File.Delete(existingPath);
                }
            }

            //WebUI projesinin wwwroot'u
            var webUiRoot = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "DogusCay.WebUI", "wwwroot");
            var folderPath = Path.Combine(webUiRoot, "uploads", "kampanyalar");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"form_{id}_{DateTime.Now.Ticks}{Path.GetExtension(image.FileName)}";
            var savePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            //Web tarafında kullanılacak yol
            var webPath = $"/uploads/kampanyalar/{fileName}";
            form.KampanyaResimYolu = webPath;

            _talepFormService.TUpdate(form);

            return Ok("Resim başarıyla yüklendi.");
        }




    }
}
