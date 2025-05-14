using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PointGrupDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointGroupsController(IPointGroupService _pointGroupService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _pointGroupService.TGetList();
            var coursePointGroups = _mapper.Map<List<ResultPointGroupDto>>(values);
            return Ok(coursePointGroups);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _pointGroupService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _pointGroupService.TDelete(id);
            return Ok("Nokta grup silindi");
        }

        [HttpPost]
        public IActionResult Create(CreatePointGroupDto createPointGroupDto)
        {
            var newValue = _mapper.Map<PointGroup>(createPointGroupDto);
            _pointGroupService.TCreate(newValue);
            return Ok(" Nokta grup Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdatePointGroupDto updatePointGroupDto)
        {
            var value = _mapper.Map<PointGroup>(updatePointGroupDto);
            _pointGroupService.TUpdate(value);
            return Ok("Nokta grup Güncellendi");
        }

        [AllowAnonymous]
        [HttpGet("GetPointGroupCount")]
        public IActionResult GetPointGroupCount()
        {
            var courseCount = _pointGroupService.TCount();
            return Ok(courseCount);
        }
        //nokta grublarını kanalı ile berraber çeker
        [AllowAnonymous]
        [HttpGet("with-kanal")]
        public IActionResult GetPointGroupsWithKanal()
        {
            var values = _pointGroupService.TGetPointGroupsWithKanal();
            return Ok(values);
        }
        //kanala bağlı nokta gruplarını getir
        [AllowAnonymous]
        [HttpGet("by-kanal/{kanalId}")]
        public IActionResult GetByKanal(int kanalId)
        {
            var values = _pointGroupService.TGetByKanalId(kanalId);
            var result = _mapper.Map<List<ResultPointGroupDto>>(values);
            return Ok(result);
        }
    }
}
