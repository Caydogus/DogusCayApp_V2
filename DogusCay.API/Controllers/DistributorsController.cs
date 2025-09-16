using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.DistributorDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;


namespace DogusCay.API.Controllers
{
    [Authorize] 
    [Route("api/distributors")]
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

        private int GetUserId()
        {
            var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        [HttpGet]
        public IActionResult Get()
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı kimliği belirlenemedi.");

            List<Distributor> values;

            if (IsAdmin())
            {
                values = _distributorService.TGetListWithAppUser();
            }
            else
            {
                values = _distributorService.TGetDistributorsByAppUserId(userId);
            }

            var result = _mapper.Map<List<ResultDistributorDto>>(values);
            return Ok(result);
        }

        // Açılır liste (dropdown) için kullanıcının rolüne göre filtrelenmiş distribütör listesini getirir.
        [HttpGet("dropdown")]
        public IActionResult GetDropdown()
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı kimliği belirlenemedi.");

            List<Distributor> distributors;

            if (IsAdmin())
            {
                distributors = _distributorService.TGetList(); // Yönetici için hepsini getir
            }
            else
            {
                distributors = _distributorService.TGetDistributorsByAppUserId(userId); // Yalnızca kullanıcının distribütörlerini getir
            }
            var list = distributors.Select(x => new DistributorDropdownDto
            {
                DistributorId = x.DistributorId,
                DistributorName = x.DistributorName
            }).ToList();

            return Ok(list);
        }

        //ID'ye göre tek bir distribütör getirir ve yetkilendirme kontrolleri yapar.
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı kimliği belirlenemedi.");

            Distributor value;

            // Adminse her distribütörü görebilir, değilse sadece kendi distribütörünü
            if (IsAdmin())
            {
                value = _distributorService.TGetById(id);
            }
            else
            {
                value = _distributorService.TGetByFilter(d => d.DistributorId == id && d.AppUserId == userId);
            }

            if (value == null)
            {
                return NotFound("Distributor bulunamadı veya bu distribütöre erişim yetkiniz yok.");
            }

            return Ok(value);
        }

        // Kullanıcının rolüne göre toplam distribütör sayısını döndürür.
        [HttpGet("count")]
        public IActionResult GetDistributorCount()
        {
            int count;
            if (IsAdmin())
            {
                count = _distributorService.TCount(); // Tüm distribütörlerin sayısını al
            }
            else
            {
                int userId = GetUserId();
                if (userId == 0)
                    return Unauthorized("Kullanıcı kimliği belirlenemedi.");
                count = _distributorService.TFilteredCount(d => d.AppUserId == userId); //generic serviste filtreleme yapıldı
            }
            return Ok(count);
        }

        // Kanal ID'sine bağlı distribütörleri getirir ve yetkilendirme kontrolleri yapar.
        [HttpGet("by-kanal/{kanalId}")]
        public IActionResult GetByKanal(int kanalId)
        {
            List<Distributor> list;

            // Kanal ID'ye göre ilk filtreleme
            list = _distributorService.TGetDistributorsByKanalId(kanalId);

            // Eğer kullanıcı Admin değilse, sadece kendi erişebileceği distribütörleri filtrele
            if (!IsAdmin())
            {
                int userId = GetUserId();
                if (userId == 0) return Unauthorized("Kullanıcı kimliği belirlenemedi.");
                list = list.Where(d => d.AppUserId == userId).ToList();
            }

            var result = list.Select(x => new
            {
                x.DistributorId,
                x.DistributorName
            }).ToList();

            return Ok(result);
        }
    }
}



