using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.ChannelDtos;
using DogusCay.DTO.DTOs.RegionDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class RegionsController(IRegionService _regionService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _regionService.TGetList();
            var courseRegions = _mapper.Map<List<ResultRegionDto>>(values);
            return Ok(courseRegions);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _regionService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _regionService.TDelete(id);
            return Ok("Bölge Silindi");
        }

        [HttpPost]
        public IActionResult Create(CreateRegionDto createRegionDto)
        {
            var newValue = _mapper.Map<Region>(createRegionDto);
            _regionService.TCreate(newValue);
            return Ok(" Bölge Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdateRegionDto updateRegionDto)
        {
            var value = _mapper.Map<Region>(updateRegionDto);
            _regionService.TUpdate(value);
            return Ok("Bölge Güncellendi");
        }

        //[AllowAnonymous]
        //[HttpGet("GetRegionCount")]
        //public IActionResult GetRegionCount()
        //{
        //    var courseCount = _regionService.TCount();
        //    return Ok(courseCount);
        //}
    }

}
