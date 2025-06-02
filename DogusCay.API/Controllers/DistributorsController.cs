using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.DistributorDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorsController : ControllerBase
    {
        private readonly IDistributorService _distributorService;
        private readonly IMapper _mapper;

        public DistributorsController(IDistributorService distributorService, IMapper mapper)
        {
            _distributorService = distributorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _distributorService.TGetListWithAppUser(); // AppUser dahil getir
            var result = _mapper.Map<List<ResultDistributorDto>>(values);
            return Ok(result);
        }
        [HttpGet("dropdown")]
        public IActionResult GetDropdown()
        {
            var list = _distributorService.TGetList()
                .Select(x => new DistributorDropdownDto
                {
                    DistributorId = x.DistributorId,
                    DistributorName = x.DistributorName
                }).ToList();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var value = _distributorService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _distributorService.TDelete(id);
            return Ok("Distributor silindi.");
        }

        [HttpPost]
        public IActionResult Create(CreateDistributorDto createDistributorDto)
        {
            var newValue = _mapper.Map<Distributor>(createDistributorDto);
            _distributorService.TCreate(newValue);
            return Ok("Distributor oluşturuldu.");
        }

        [HttpPut]
        public IActionResult Update(UpdateDistributorDto updateDistributorDto)
        {
            var value = _mapper.Map<Distributor>(updateDistributorDto);
            _distributorService.TUpdate(value);
            return Ok("Distributor güncellendi.");
        }

        [AllowAnonymous]
        [HttpGet("GetDistributorCount")]
        public IActionResult GetDistributorCount()
        {
            var count = _distributorService.TCount();
            return Ok(count);
        }

        //KanalId'ye bağlı olan tüm distributorları getirir.
        [AllowAnonymous]
        [HttpGet("by-kanal/{kanalId}")]
        public IActionResult GetByKanal(int kanalId)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var list = _distributorService.TGetDistributorsByKanalId(kanalId);
            var result = list.Select(x => new
            {
                DistributorId = x.DistributorId,
                DistributorName = x.DistributorName
            }).ToList();

            return Ok(result);
        }


    }
}
