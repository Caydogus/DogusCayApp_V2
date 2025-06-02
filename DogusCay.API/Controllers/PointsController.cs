
using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase 
    {
        private readonly IPointService _pointService;
        private readonly IMapper _mapper;
        private readonly IKanalService _kanalService;
        private readonly IDistributorService _distributorService;

        public PointsController(IPointService pointService, IMapper mapper, IKanalService kanalService, IDistributorService distributorService)
        {
            _pointService = pointService;
            _mapper = mapper;
            _kanalService = kanalService;
            _distributorService = distributorService;
        }

        [HttpGet]
       // [Authorize] // Sadece giriş yapan kullanıcılar
        public IActionResult Get()
        {

            var values = _pointService.TGetListWithIncludes();

            var result = values.Select(p => new ResultPointDto
            {
                PointId = p.PointId,
                PointErc = p.PointErc,
                PointName = p.PointName,
                KanalName = p.Kanal?.KanalName,
                DistributorName = p.Distributor?.DistributorName,
                PointGroupTypeName = p.PointGroupType?.PointGroupTypeName,
                AppUserFullName = $"{p.AppUser?.FirstName} {p.AppUser?.LastName}"
            }).ToList();

            return Ok(result);
            ////  Giriş yapan kullanıcının ID'sini al
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ////  Sadece bu kullanıcıya ait noktaları çek
            //var values = _pointService.TGetFilteredList(p => p.AppUserId == userId);

            //var points = _mapper.Map<List<ResultPointDto>>(values);
            //return Ok(points);
        }

        [HttpGet("{id}")]
       //[Authorize]
        public IActionResult GetById(int id)
        {
            //Giriş yapan kullanıcı sadece kendi noktasını görebilsin
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var value = _pointService.TGetByFilter(p => p.PointId == id && p.AppUserId == userId);

            if (value == null)
                return NotFound("Bu noktaya erişim yetkiniz yok.");

            var dto = _mapper.Map<ResultPointDto>(value);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
      //  [Authorize]
        public IActionResult Delete(int id)
        {
            //Silme yetkisi sadece kendi noktasına olmalı
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var point = _pointService.TGetByFilter(p => p.PointId == id && p.AppUserId == userId);

            if (point == null)
                return NotFound("Bu noktayı silme yetkiniz yok.");

            _pointService.TDelete(id);
            return Ok("Nokta silindi.");
        }

        [HttpPost]
        //  [Authorize]
        [HttpPost]
        public IActionResult Create(CreatePointDto createPointDto)
        {
            if (createPointDto == null)
                return BadRequest("Veri eksik.");

            var newPoint = _mapper.Map<Point>(createPointDto);

            // Kanalı bul (adıyla kontrol için)
            var kanal = _kanalService.TGetById(createPointDto.KanalId);

            if (kanal == null)
                return BadRequest("Geçersiz KanalId");

            if (kanal.KanalName == "DIST")
            {
                if (!createPointDto.DistributorId.HasValue)
                    return BadRequest("DIST kanalı için DistributorId zorunlu.");

                var distributor = _distributorService.TGetById(createPointDto.DistributorId.Value);
                if (distributor == null)
                    return NotFound("Distributor bulunamadı.");

                newPoint.AppUserId = distributor.AppUserId;
            }
            else // LC veya NA
            {
                if (createPointDto.AppUserId == 0)
                    return BadRequest("LC veya NA kanalı için AppUserId gönderilmelidir.");

                newPoint.AppUserId = createPointDto.AppUserId;
            }

            _pointService.TCreate(newPoint);
            return Ok("Nokta oluşturuldu.");
        }

        [HttpPut]
       // [Authorize]
        public IActionResult Update(UpdatePointDto updatePointDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //Noktanın sahibi kullanıcı mı, kontrol et
            var existing = _pointService.TGetByFilter(p => p.PointId == updatePointDto.PointId && p.AppUserId == userId);

            if (existing == null)
                return NotFound("Bu noktayı güncelleme yetkiniz yok.");

            var updatedPoint = _mapper.Map<Point>(updatePointDto);
            updatedPoint.AppUserId = userId; // Güvenlik için tekrar set et

            _pointService.TUpdate(updatedPoint);
            return Ok("Nokta güncellendi.");
        }

        [HttpGet("GetPointCount")]
       // [Authorize]
        public IActionResult GetPointCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //Sadece kendi noktalarının sayısı
            var count = _pointService.TFilteredCount(p => p.AppUserId == userId);
            return Ok(count);
        }


        [AllowAnonymous]
        //NA / LC gibi distributor olmayan kanallar için.KanalId ile doğrudan noktaları getirir.
        [HttpGet("by-kanal/{kanalId}")]
        public IActionResult GetByKanal(int kanalId)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var list = _pointService.TGetByKanalId(kanalId);
            var result = _mapper.Map<List<ResultPointDto>>(list);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("dropdown")]
        public IActionResult GetDropdown()
        {
           
            var list = _pointService.TGetList()
                .Select(p => new PointDropdownDto
                {
                    PointId = p.PointId,
                    PointName = p.PointName
                }).ToList();

            return Ok(list);
        }
        ////lc ve na için
        //[AllowAnonymous]
        //[HttpGet("by-group/{pointGroupTypeId}")]
        //public IActionResult GetPointsByGroup(int pointGroupTypeId)
        //{
        //    var list = _pointService.TGetFilteredList(p => p.PointGroupTypeId == pointGroupTypeId);
        //    var result = _mapper.Map<List<ResultPointDto>>(list);
        //    return Ok(result);
        //}


        //PointGroupTypeId + DistributorId’ye göre noktaları getirir.DIST kanalına ait zincir içindir.

        [HttpGet("by-group/{pointGroupTypeId}/distributor/{distributorId}")]
        public IActionResult GetByGroupAndDistributor(int distributorId, int pointGroupTypeId)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var list = _pointService.TGetByDistributorAndGroup(distributorId, pointGroupTypeId);
            var result = _mapper.Map<List<ResultPointDto>>(list);
            return Ok(result);
        }

    }
}


