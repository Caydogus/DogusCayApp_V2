using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [AllowAnonymous] // Test modu için
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
      
        // 🔹 Kullanıcının kendi talepleri
        [HttpGet("mine")]
        // [Authorize(Roles = "BolgeMuduru")]
        public IActionResult GetOwnForms()
        {
            int userId = 9; // TEST kullanıcısı
            var result = _talepFormService.TGetAllByUserId(userId);
            return Ok(result);
        }

        // 🔹 Admin: tüm talepler
        [HttpGet]
        // [Authorize(Roles = "Admin")]
       
        public IActionResult GetAllForms()
        {
            var result = _talepFormService.TGetAllWithUser();
            var dtoList = _mapper.Map<List<ResultTalepFormDto>>(result);
            return Ok(dtoList);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTalepFormDto dto)
        {
            Console.WriteLine("🚀 API'ye veri ulaştı:");
            Console.WriteLine($"ProductId: {dto.ProductId}, Quantity: {dto.Quantity}, Total: {dto.Total}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("❌ Validation Hataları: " + string.Join(", ", errors));
                return BadRequest(ModelState);
            }

            Console.WriteLine($"📥 DTO: ProductId={dto.ProductId}, PointId={dto.PointId}, KanalId={dto.KanalId}");

            var entity = new TalepForm
            {
                AppUserId = dto.AppUserId ?? 9,
                KanalId = dto.KanalId,
                DistributorId = dto.DistributorId,
                PointGroupTypeId = dto.PointGroupTypeId,
                PointId = dto.PointId ?? 0,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                SubSubCategoryId = dto.SubSubCategoryId,
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                ErpCode = dto.ErpCode,
                Quantity = dto.Quantity <= 0 ? 1 : dto.Quantity,
                Price = dto.Price,
                KoliIciAdet = dto.KoliIciAdet,
                ApproximateWeightKg = dto.ApproximateWeightKg,
                SabitBedelTL = dto.SabitBedelTL,
                Note = dto.Note,
                ValidFrom = dto.ValidFrom < new DateTime(1753, 1, 1) ? DateTime.Now : dto.ValidFrom,
                ValidTo = dto.ValidTo < new DateTime(1753, 1, 1) ? DateTime.Now.AddDays(7) : dto.ValidTo,
                Iskonto1 = dto.Iskonto1,
                Iskonto2 = dto.Iskonto2,
                Iskonto3 = dto.Iskonto3,
                Iskonto4 = dto.Iskonto4,
                AdetFarkDonusuTL = dto.AdetFarkDonusuTL,
                TalepTip = TalepTip.Insert,
            };

            // Hesaplamalar
            entity.KoliIciToplamAdet = entity.Quantity * dto.KoliIciAdet;
            entity.KoliToplamAgirligiKg = entity.Quantity * dto.ApproximateWeightKg;
            entity.ListeFiyat = dto.KoliIciAdet > 0 ? dto.Price / dto.KoliIciAdet : 0;

            decimal toplam = dto.Price * entity.Quantity;
            if (dto.Iskonto1 > 0) toplam *= (100 - dto.Iskonto1) / 100m;
            if (dto.Iskonto2 > 0) toplam *= (100 - dto.Iskonto2) / 100m;
            if (dto.Iskonto3 > 0) toplam *= (100 - dto.Iskonto3) / 100m;
            if (dto.Iskonto4 > 0) toplam *= (100 - dto.Iskonto4) / 100m;

            var calculated = entity.KoliIciToplamAdet > 0 ? Math.Round(toplam / entity.KoliIciToplamAdet, 2) : 0;

            if (dto.AdetFarkDonusuTL > 0)
            {
                entity.SonAdetFiyati = Math.Round(calculated - dto.AdetFarkDonusuTL, 2);
                entity.Total = Math.Round(entity.SonAdetFiyati * entity.KoliIciToplamAdet, 2);
            }
            else
            {
                entity.SonAdetFiyati = calculated;
                entity.Total = Math.Round(toplam, 2);
            }

            _talepFormService.TCreate(entity);

            return Ok("Talep başarıyla oluşturuldu.");
        }

        [HttpGet("{id}")]
        // [Authorize]
        public IActionResult GetDetails(int id)
        {
            var entity = _talepFormService.TGetDetailsForForm(id);
            if (entity == null) return NotFound("Talep bulunamadı.");
            var dto = _mapper.Map<ResultTalepFormDto>(entity);
            return Ok(dto);
        }

        // 🔹 Admin onaylama
        [HttpPost("approve/{id}")]
        // [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            int adminId = 10; // Test admin ID
            _talepFormService.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);
            return Ok("Talep onaylandı.");
        }

        // 🔹 Admin reddetme
        [HttpPost("reject/{id}")]
        // [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            int adminId = 10; // Test admin ID
            _talepFormService.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);
            return Ok("Talep reddedildi.");
        }

        // 🔹 Admin tüm formu günceller
        [HttpPut]
        // [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] UpdateTalepFormDto dto)
        {
            var entity = _mapper.Map<TalepForm>(dto);
            _talepFormService.TUpdate(entity);
            return Ok("Talep güncellendi.");
        }

        // 🔹 Bölge Müdürü: sadece ürün miktarı ve tarihleri günceller
        [HttpPatch("update-items")]
        // [Authorize(Roles = "BolgeMuduru")]
        public IActionResult UpdateItemFields([FromBody] UpdateTalepFormDto dto)
        {
            foreach (var item in dto.Items)
            {
                _talepFormService.TUpdateItemFields(
                    item.TalepFormItemId,
                    item.Quantity,
                    item.ValidFrom,
                    item.ValidTo
                );
            }
            return Ok("Ürün bilgileri güncellendi.");
        }

        // 🔹 Kendi onaylanmamış formunu silebilir
        [HttpDelete("{id}")]
        // [Authorize(Roles = "BolgeMuduru")]
       
        public IActionResult Delete(int id)
        {
            int userId = 9; // TEST kullanıcısı
            var form = _talepFormService.TGetById(id);

            if (form == null || form.AppUserId != userId || form.TalepDurumu != TalepDurumu.Bekliyor)
                return BadRequest("Bu talep silinemez.");

            _talepFormService.TDelete(id);
            return Ok("Talep silindi.");
        }


        // 🔹 Toplam form sayısı
        [HttpGet("count")]
        public IActionResult GetFormCount()
        {
            var count = _talepFormService.TCount();
            return Ok(count);
        }
    }
}


