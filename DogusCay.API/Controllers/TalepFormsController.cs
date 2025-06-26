using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{

    [Authorize] // Yetkilendirme gerektiren controller
    [Route("api/[controller]")]
    [ApiController]
    public class TalepFormsController : ControllerBase
    {
        private readonly ITalepFormService _talepFormService;
        private readonly IMapper _mapper;

        public TalepFormsController(ITalepFormService talepFormService, IMapper mapper)
        {
            _talepFormService = talepFormService;
            _mapper = mapper;
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
        public IActionResult GetOwnFormsPaged(int page = 1, int pageSize = 10)
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
        public IActionResult GetAllFormsPaged(int page = 1, int pageSize = 10)
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

        // Yeni bir talep formu oluşturur.
        [Authorize] // Sadece giriş yapmış kullanıcılar talep oluşturabilir
        [HttpPost]
        public IActionResult Create([FromBody] CreateTalepFormDto dto)
        {
            Console.WriteLine("Claims:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            Console.WriteLine("API'ye veri ulaştı.");
            Console.WriteLine($"ProductId: {dto.ProductId}, Quantity: {dto.Quantity}, Total: {dto.Total}");

            // JWT token'dan gelen kullanıcının ID'sini doğrudan al
            int authenticatedUserId = GetUserId();
            if (authenticatedUserId == 0)
            {
                // Kullanıcı ID'si alınamazsa yetkilendirme hatası dön
                Console.WriteLine("❌ Yetkilendirilmiş kullanıcı ID'si bulunamadı.");
                return Unauthorized("Kullanıcı ID'si token'dan alınamadı. Lütfen giriş yapın.");
            }
            Console.WriteLine("API'den alınan AppUserId (Token'dan): " + authenticatedUserId);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("❌ Validation Hataları: " + string.Join(", ", errors));
                return BadRequest(ModelState);
            }

            Console.WriteLine($"DTO: ProductId={dto.ProductId}, PointId={dto.PointId}, KanalId={dto.KanalId}");

            var entity = _mapper.Map<TalepForm>(dto); // DTO'yu entity'ye map'le

            // KRİTİK GÜNCELLEME: AppUserId'yi doğrudan JWT token'dan alınan ID ile set et!**
            entity.AppUserId = authenticatedUserId;

            entity.Quantity = dto.Quantity <= 0 ? 1 : dto.Quantity;
            entity.PointId = dto.PointId ?? 0;
            entity.ValidFrom = dto.ValidFrom < new DateTime(1753, 1, 1) ? DateTime.Now : dto.ValidFrom;
            entity.ValidTo = dto.ValidTo < new DateTime(1753, 1, 1) ? DateTime.Now.AddDays(7) : dto.ValidTo;
            entity.TalepTip = TalepTip.Insert; // Bu sabit kalmalı

            Console.WriteLine(" INSERT için kullanılacak AppUserId (Entity içinde): " + entity.AppUserId);

            // Hesaplamalar
            entity.BrutTotal = entity.Quantity * dto.Price;//yeni eklendi


            entity.KoliIciToplamAdet = entity.Quantity * dto.KoliIciAdet;
            entity.KoliToplamAgirligiKg = entity.Quantity * dto.ApproximateWeightKg;
            entity.ListeFiyat = dto.KoliIciAdet > 0 ? dto.Price / dto.KoliIciAdet : 0;

            decimal toplam = dto.Price * entity.Quantity;

            // 2. İskontoları sırayla uygula
            if (dto.Iskonto1 > 0) toplam *= (100 - dto.Iskonto1) / 100m;
            if (dto.Iskonto2 > 0) toplam *= (100 - dto.Iskonto2) / 100m;
            if (dto.Iskonto3 > 0) toplam *= (100 - dto.Iskonto3) / 100m;
            if (dto.Iskonto4 > 0) toplam *= (100 - dto.Iskonto4) / 100m;

            // 3. Sabit bedeli çıkar
            if (dto.SabitBedelTL > 0)
            {
                toplam -= (decimal)dto.SabitBedelTL;
                if (toplam < 0) toplam = 0;
            }

            // 4. Son adet fiyatı: toplam / koli içi toplam adet
            decimal sonAdetFiyat = entity.KoliIciToplamAdet > 0 ? toplam / entity.KoliIciToplamAdet : 0;

            // 5. Adet farkı varsa, bundan da düş
            if (dto.AdetFarkDonusuTL > 0)
            {
                sonAdetFiyat -= dto.AdetFarkDonusuTL;
                if (sonAdetFiyat < 0) sonAdetFiyat = 0;
            }

            // 6. Toplamı tekrar hesapla
            decimal finalToplam = Math.Round(sonAdetFiyat * entity.KoliIciToplamAdet, 2);

            // 7. Ata
            entity.SonAdetFiyati = Math.Round(sonAdetFiyat, 2);
            entity.Total = finalToplam;

            //maliyeti yuzde(%) cinsinden hesapla
            entity.Maliyet = entity.BrutTotal != 0
            ? Math.Round(((entity.BrutTotal - entity.Total) / entity.BrutTotal) * 100, 2)
            : 0;


            _talepFormService.TCreate(entity);

            Console.WriteLine($"Toplam (İskonto ve sabit bedel sonrası): {toplam}");
            Console.WriteLine($"SonAdetFiyati (Adet fark sonrası): {entity.SonAdetFiyati}");
            Console.WriteLine($"Final Total: {entity.Total}");

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
        [Authorize(Roles = "Admin")] // Sadece Admin rolündekiler erişebilir
        public IActionResult Approve(int id)
        {
            // Onaylayan Admin'in ID'sini JWT token'dan al
            int adminId = GetUserId();
            if (adminId == 0)
            {
                return Unauthorized("Admin ID'si token'dan alınamadı.");
            }
            _talepFormService.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);
            return Ok("Talep onaylandı.");
        }


        // Talep formunu reddeder (Admin rolü için).
        [HttpPost("reject/{id}")]
        [Authorize(Roles = "Admin")] // Sadece Admin rolündekiler erişebilir
        public IActionResult Reject(int id)
        {
            // Reddeden Admin'in ID'sini JWT token'dan al
            int adminId = GetUserId();
            if (adminId == 0)
            {
                return Unauthorized("Admin ID'si token'dan alınamadı.");
            }
            _talepFormService.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);
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

            // 🔥 Eski resmi sil
            if (!string.IsNullOrEmpty(form.KampanyaResimYolu))
            {
                var existingPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "DogusCay.WebUI", "wwwroot", form.KampanyaResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(existingPath))
                {
                    System.IO.File.Delete(existingPath);
                }
            }

            // 📂 WebUI projesinin wwwroot'u
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

            // 🌐 Web tarafında kullanılacak yol
            var webPath = $"/uploads/kampanyalar/{fileName}";
            form.KampanyaResimYolu = webPath;

            _talepFormService.TUpdate(form);

            return Ok("Resim başarıyla yüklendi.");
        }




    }
}
