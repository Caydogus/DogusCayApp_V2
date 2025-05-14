
using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase 
    {
        private readonly IPointService _pointService;
        private readonly IMapper _mapper;

        public PointsController(IPointService pointService, IMapper mapper)
        {
            _pointService = pointService;
            _mapper = mapper;
        }

        [HttpGet]
       // [Authorize] // Sadece giriş yapan kullanıcılar
        public IActionResult Get()
        {
            //  Giriş yapan kullanıcının ID'sini al
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //  Sadece bu kullanıcıya ait noktaları çek
            var values = _pointService.TGetFilteredList(p => p.AppUserId == userId);

            var coursePoints = _mapper.Map<List<ResultPointDto>>(values);
            return Ok(coursePoints);
        }

        [HttpGet("{id}")]
       // [Authorize]
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
        public IActionResult Create(CreatePointDto createPointDto)
        {
            //Giriş yapan kullanıcı ID'si atanır
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var newPoint = _mapper.Map<Point>(createPointDto);
            newPoint.AppUserId = userId; // DTO'dan gelmesini engelle, burada set et
            newPoint.CreatedDate = DateTime.Now;

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
        //seçilen nokta grubuna bağlı noktalar gelecek
        [AllowAnonymous]
        [HttpGet("by-pointgroup/{pointGroupId}")]
        public IActionResult GetByPointGroup(int pointGroupId)
        {
            var values = _pointService.TGetByPointGroupId(pointGroupId);
            var result = _mapper.Map<List<ResultPointDto>>(values);
            return Ok(result);
        }
    }
}


#region
//using AutoMapper;
//using DogusCay.Business.Abstract;
//using DogusCay.DTO.DTOs.PointDtos;
//using DogusCay.Entity.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace DogusCay.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PointsController(IPointService _pointService, IMapper _mapper) : ControllerBase
//    {
//        [AllowAnonymous]
//        [HttpGet]
//        public IActionResult Get()
//        {
//            var values = _pointService.TGetList();
//            var coursePoints = _mapper.Map<List<ResultPointDto>>(values);
//            return Ok(coursePoints);
//        }

//        [HttpGet("{id}")]

//        public IActionResult GetById(int id)
//        {
//            var value = _pointService.TGetById(id);
//            return Ok(value);
//        }

//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            _pointService.TDelete(id);
//            return Ok("Nokta  silindi");
//        }

//        [HttpPost]
//        public IActionResult Create(CreatePointDto createPointDto)
//        {
//            var newValue = _mapper.Map<Point>(createPointDto);
//            _pointService.TCreate(newValue);
//            return Ok(" Nokta  Oluşturuldu");
//        }

//        [HttpPut]
//        public IActionResult Update(UpdatePointDto updatePointDto)
//        {
//            var value = _mapper.Map<Point>(updatePointDto);
//            _pointService.TUpdate(value);
//            return Ok("Nokta Güncellendi");
//        }

//        [AllowAnonymous]
//        [HttpGet("GetPointCount")]
//        public IActionResult GetPointCount()
//        {
//            var courseCount = _pointService.TCount();
//            return Ok(courseCount);
//        }


//    }
//}
#endregion