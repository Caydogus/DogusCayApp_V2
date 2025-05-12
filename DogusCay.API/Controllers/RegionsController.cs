using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DTO.DTOs.ChannelDtos;
using DogusCay.DTO.DTOs.RegionDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly DogusCayContext _context; // veya senin DbContext adın neyse

        public RegionsController(IRegionService regionService,IMapper mapper,UserManager<AppUser> userManager,DogusCayContext context)
        {
            _regionService = regionService;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }
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

        //bolgeleri bolge mudurleriyle beraber çek
        [AllowAnonymous]
        [HttpGet("WithManager")]
        public async Task<IActionResult> GetRegionsWithManager()
        {
            var regions = await _context.Regions.Include(r => r.ManagerUser).ToListAsync();

            var result = new List<ResultRegionDto>();

            foreach (var region in regions)
            {
                var dto = new ResultRegionDto
                {
                    RegionId = region.RegionId,
                    RegionName = region.RegionName,
                    ManagerUserId = region.ManagerUserId
                };

                if (region.ManagerUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(region.ManagerUser);
                    if (roles.Contains("BolgeMuduru"))
                    {
                        dto.ManagerFirstName = region.ManagerUser.FirstName;
                        dto.ManagerLastName = region.ManagerUser.LastName;
                    }
                }

                result.Add(dto);
            }

            return Ok(result);
        }


    }

}
