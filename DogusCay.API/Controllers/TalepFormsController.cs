using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalepFormsController : ControllerBase
    {
        private readonly ITalepFormService _talepFormService;
        private readonly IMapper _mapper;
        private readonly ILogger<TalepFormsController> _logger;

        public TalepFormsController(ITalepFormService talepFormService, IMapper mapper, ILogger<TalepFormsController> logger)
        {
            _talepFormService = talepFormService;
            _mapper = mapper;
            _logger = logger;
        }

        // 🔹 Bölge müdürü kendi taleplerini görsün
        [HttpGet("bolgemuduru")]
        //[Authorize(Roles = "BolgeMuduru")]
        public IActionResult GetByUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var values = _talepFormService.TGetAllByUserId(userId);
            return Ok(values);
        }

        // 🔹 Admin tüm talepleri görsün
        [HttpGet("admin")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var values = _talepFormService.TGetAllWithUser();
            return Ok(values);
        }

        // 🔹 Talep oluştur (bölge müdürü)
        [HttpPost]
        //[Authorize(Roles = "BolgeMuduru")]
        public IActionResult Create(CreateTalepFormDto createTalepFormDto)
        {
            var entity = _mapper.Map<TalepForm>(createTalepFormDto);
            _talepFormService.TCreate(entity);
            return Ok("Talep başarıyla oluşturuldu");
        }

        // 🔹 Talebi onayla (admin)
        [HttpPut("approve/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            int adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _talepFormService.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);
            return Ok("Talep onaylandı");
        }

        // 🔹 Talebi reddet (admin)
        [HttpPut("reject/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            int adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _talepFormService.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);
            return Ok("Talep reddedildi");
        }

        // 🔹 Talebi güncelle (admin: tüm alanlar)
        [HttpPut("update")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Update(UpdateTalepFormDto updateTalepFormDto)
        {
            var entity = _mapper.Map<TalepForm>(updateTalepFormDto);
            _talepFormService.TUpdate(entity);
            return Ok("Talep güncellendi (admin)");
        }

        // 🔹 Ürün miktarı ve tarihlerini güncelle (bölge müdürü)
        [HttpPut("update-quantity")]
        //[Authorize(Roles = "BolgeMuduru")]
        public IActionResult UpdateQuantity(UpdateTalepFormDto updateTalepFormDto)
        {
            foreach (var item in updateTalepFormDto.Items)
            {
                _talepFormService.TUpdateItemFields(
                    item.TalepFormItemId,
                    item.Quantity,
                    item.ValidFrom,
                    item.ValidTo
                );
            }
            return Ok("Ürün adet ve tarihleri güncellendi (bölge müdürü)");
        }

        // 🔹 Talebi sil (onaylanmamışsa ve kullanıcıya aitse)
        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "BolgeMuduru")]
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var form = _talepFormService.TGetById(id);

            if (form == null || form.AppUserId != userId || form.TalepDurumu != TalepDurumu.Bekliyor)
            {
                return BadRequest("Bu talep silinemez.");
            }

            _talepFormService.TDelete(id);
            return Ok("Talep silindi");
        }

        // 🔹 Toplam talep sayısını getir
        [HttpGet("GetTalepCount")]
        [AllowAnonymous]
        public IActionResult GetTalepCount()
        {
            var count = _talepFormService.TCount();
            return Ok(count);
        }
    }
}
