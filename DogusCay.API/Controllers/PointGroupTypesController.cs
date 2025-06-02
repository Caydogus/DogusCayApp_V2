using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PointGroupTypeDtos;
using DogusCay.DTO.DTOs.PointGrupDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointGroupTypesController(IPointGroupTypeService _pointGroupTypeService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _pointGroupTypeService.TGetList();
            var pointGroupTypes = _mapper.Map<List<ResultPointGroupTypeDto>>(values);
            return Ok(pointGroupTypes);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _pointGroupTypeService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _pointGroupTypeService.TDelete(id);
            return Ok("Nokta grup silindi");
        }

        [HttpPost]
        public IActionResult Create(CreatePointGroupTypeDto createPointGroupTypeDto)
        {
            var newValue = _mapper.Map<PointGroupType>(createPointGroupTypeDto);
            _pointGroupTypeService.TCreate(newValue);
            return Ok(" Nokta grup Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdatePointGroupTypeDto updatePointGroupTypeDto)
        {
            var value = _mapper.Map<PointGroupType>(updatePointGroupTypeDto);
            _pointGroupTypeService.TUpdate(value);
            return Ok("Nokta grup Güncellendi");
        }

        //Bir distributora bağlı PointGroupType listesini getiriyor..
        //sistemdeki Talep Formu Zinciri için gerekli.
        [HttpGet("by-distributor/{distributorId}")]
        public IActionResult GetByDistributor(int distributorId)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var list = _pointGroupTypeService.TGetPointGroupsByDistributorId(distributorId);
            var result = _mapper.Map<List<ResultPointGroupTypeDto>>(list);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("dropdown")]
        public IActionResult GetDropdown()
        {
          
            var list = _pointGroupTypeService.TGetList()
                .Select(pg => new PointGroupTypeDropdownDto
                {
                    PointGroupTypeId = pg.PointGroupTypeId,
                    PointGroupTypeName = pg.PointGroupTypeName
                }).ToList();

            return Ok(list);
        }

    }
}
