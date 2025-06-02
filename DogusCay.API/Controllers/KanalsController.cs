using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.ChannelDtos;
using DogusCay.DTO.DTOs.KanalDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KanalsController(IKanalService _kanalService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var kanallar = _kanalService.TGetList();
            var result = _mapper.Map<List<ResultKanalDto>>(kanallar);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("dropdown")]
        public IActionResult GetDropdown()
        {
            var list = _kanalService.TGetList()
                .Select(k => new KanalDropdownDto
                {
                    KanalId = k.KanalId,
                    KanalName = k.KanalName
                }).ToList();

            return Ok(list);
        }
        
        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _kanalService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _kanalService.TDelete(id);
            return Ok("Kanal Alanı Silindi");
        }

        [HttpPost]
        public IActionResult Create(CreateKanalDto createKanalDto)
        {
            var newValue = _mapper.Map<Kanal>(createKanalDto);
            _kanalService.TCreate(newValue);
            return Ok(" Kanal Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdateKanalDto updateKanalDto)
        {
            var value = _mapper.Map<Kanal>(updateKanalDto);
            _kanalService.TUpdate(value);
            return Ok("Kanal Güncellendi");
        }
   
        [AllowAnonymous]
        [HttpGet("GetKanalCount")]
        public IActionResult GetKanalCount()
        {
            var courseCount = _kanalService.TCount();
            return Ok(courseCount);
        }


    }
}
