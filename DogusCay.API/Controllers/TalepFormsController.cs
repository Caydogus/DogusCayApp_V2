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
            return Ok(result);
        }

        // 🔹 Form oluşturma
        [HttpPost]
        // [Authorize(Roles = "BolgeMuduru")]
        public IActionResult Create([FromBody] CreateTalepFormDto dto)
        {
            var entity = _mapper.Map<TalepForm>(dto);
            _talepFormService.TCreate(entity);
            return Ok("Talep başarıyla oluşturuldu.");
        }

        // 🔹 Detay (form + ürünler)
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


//using AutoMapper;
//using DogusCay.Business.Abstract;
//using DogusCay.DTO.DTOs.TalepFormDtos;
//using DogusCay.Entity.Entities.Talep;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace DogusCay.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TalepFormsController : ControllerBase
//    {
//        private readonly ITalepFormService _talepFormService;
//        private readonly IMapper _mapper;

//        public TalepFormsController(ITalepFormService talepFormService, IMapper mapper)
//        {
//            _talepFormService = talepFormService;
//            _mapper = mapper;
//        }

//        // 🔹 Kullanıcının kendi talepleri
//        [HttpGet("mine")]
//        [Authorize(Roles = "BolgeMuduru")]
//        public IActionResult GetOwnForms()
//        {
//            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//            var result = _talepFormService.TGetAllByUserId(userId);
//            return Ok(result);
//        }

//        // 🔹 Admin: tüm talepler
//        [HttpGet]
//        [Authorize(Roles = "Admin")]
//        public IActionResult GetAllForms()
//        {
//            var result = _talepFormService.TGetAllWithUser();
//            return Ok(result);
//        }

//        // 🔹 Form oluşturma
//        [HttpPost]
//        [Authorize(Roles = "BolgeMuduru")]
//        public IActionResult Create([FromBody] CreateTalepFormDto dto)
//        {
//            var entity = _mapper.Map<TalepForm>(dto);
//            _talepFormService.TCreate(entity);
//            return Ok("Talep başarıyla oluşturuldu.");
//        }

//        // 🔹 Detay (form + ürünler)
//        [HttpGet("{id}")]
//        [Authorize]
//        public IActionResult GetDetails(int id)
//        {
//            var entity = _talepFormService.TGetDetailsForForm(id);
//            if (entity == null) return NotFound("Talep bulunamadı.");
//            var dto = _mapper.Map<ResultTalepFormDto>(entity);
//            return Ok(dto);
//        }

//        // 🔹 Admin onaylama
//        [HttpPost("approve/{id}")]
//        [Authorize(Roles = "Admin")]
//        public IActionResult Approve(int id)
//        {
//            int adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//            _talepFormService.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);
//            return Ok("Talep onaylandı.");
//        }

//        // 🔹 Admin reddetme
//        [HttpPost("reject/{id}")]
//        [Authorize(Roles = "Admin")]
//        public IActionResult Reject(int id)
//        {
//            int adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//            _talepFormService.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);
//            return Ok("Talep reddedildi.");
//        }

//        // 🔹 Admin tüm formu günceller
//        [HttpPut]
//        [Authorize(Roles = "Admin")]
//        public IActionResult Update([FromBody] UpdateTalepFormDto dto)
//        {
//            var entity = _mapper.Map<TalepForm>(dto);
//            _talepFormService.TUpdate(entity);
//            return Ok("Talep güncellendi.");
//        }

//        // 🔹 Bölge Müdürü: sadece ürün miktarı ve tarihleri günceller
//        [HttpPatch("update-items")]
//        [Authorize(Roles = "BolgeMuduru")]
//        public IActionResult UpdateItemFields([FromBody] UpdateTalepFormDto dto)
//        {
//            foreach (var item in dto.Items)
//            {
//                _talepFormService.TUpdateItemFields(
//                    item.TalepFormItemId,
//                    item.Quantity,
//                    item.ValidFrom,
//                    item.ValidTo
//                );
//            }
//            return Ok("Ürün bilgileri güncellendi.");
//        }

//        // 🔹 Kendi onaylanmamış formunu silebilir
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "BolgeMuduru")]
//        public IActionResult Delete(int id)
//        {
//            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//            var form = _talepFormService.TGetById(id);

//            if (form == null || form.AppUserId != userId || form.TalepDurumu != (int)TalepDurumu.Bekliyor)
//                return BadRequest("Bu talep silinemez.");

//            _talepFormService.TDelete(id);
//            return Ok("Talep silindi.");
//        }

//        // 🔹 Toplam form sayısı
//        [HttpGet("count")]
//        [AllowAnonymous]
//        public IActionResult GetFormCount()
//        {
//            var count = _talepFormService.TCount();
//            return Ok(count);
//        }
//    }
//}
