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

        
        // Kullanıcının kendi taleplerini getirir.
        [HttpGet("mine")]
        [Authorize(Roles = "BolgeMuduru")] // Sadece Bölge Müdürü rolündekiler erişebilir
        public IActionResult GetOwnForms()
        {
            int userId = GetUserId(); // JWT token'dan gelen kullanıcı ID'si
            if (userId == 0)
            {
                return Unauthorized("Kullanıcı ID'si alınamadı.");
            }
            var result = _talepFormService.TGetAllByUserId(userId);
            var dtoList = _mapper.Map<List<ResultTalepFormDto>>(result);
            return Ok(dtoList);
        }

       
        // Tüm talepleri getirir (Admin rolü için).
        [HttpGet]
        [Authorize(Roles = "Admin")] // Sadece Admin rolündekiler erişebilir
        public IActionResult GetAllForms()
        {
            var result = _talepFormService.TGetAllWithUser();
            var dtoList = _mapper.Map<List<ResultTalepFormDto>>(result);
            return Ok(dtoList);
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

        ///// <summary>
        ///// Bölge Müdürü sadece ürün miktarı ve tarihleri günceller.
        ///// </summary>
        //[HttpPatch("update-items")]
        //[Authorize(Roles = "BolgeMuduru")] // Sadece Bölge Müdürü rolündekiler erişebilir
        //public IActionResult UpdateItemFields([FromBody] UpdateTalepFormDto dto) // Eğer DTO içinde Items varsa
        //{
        //    int userId = GetUserId(); // Güncelleyen kullanıcının ID'si
        //    if (userId == 0)
        //    {
        //        return Unauthorized("Kullanıcı ID'si alınamadı.");
        //    }
        //    // Her bir TalepFormItemId için güncelleme
        //    foreach (var item in dto.Items)
        //    {
        //        _talepFormService.TUpdateItemFields(
        //            item.TalepFormItemId,
        //            item.Quantity,
        //            item.ValidFrom,
        //            item.ValidTo
        //        // Güncelleyen kullanıcı ID'si de burada parametre olarak gönderilebilir
        //        );
        //    }
        //    return Ok("Ürün bilgileri güncellendi.");
        //}

        /// <summary>
        /// Kullanıcının kendi onaylanmamış formunu silebilir.
        /// </summary>
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


    }
}