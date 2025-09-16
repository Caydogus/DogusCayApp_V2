using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq; // LINQ için eklendi

namespace DogusCay.API.Controllers
{
    [Authorize]
    [Route("api/points")]
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

        // Kullanıcının ID'sini alır
        // Bu metot User nesnesi ClaimTypes.NameIdentifier'dan okuyacaktır.
        private int GetUserId()
        {
            var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        private bool IsBolgeMuduru()
        {
            return User.IsInRole("BolgeMuduru");
        }

        //Yeni Sayfalama ve Kullanıcı Bazlı Erişim Metotları

        [HttpGet("mine-paged")]
        [Authorize(Roles = "BolgeMuduru")] // Sadece Bölge Müdürleri kendi noktalarını görebilir.
        public IActionResult GetOwnPointsPaged(int page = 1, int pageSize = 10)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı ID'si alınamadı.");

            var userPoints = _pointService.TGetListWithIncludes().AsQueryable()
                                .Where(p => p.AppUserId == userId); // Kullanıcıya ait noktaları filtrele

            var totalCount = userPoints.Count();
            var pagedData = userPoints
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = pagedData.Select(p => new ResultPointDto
            {
                PointId = p.PointId,
                PointErc = p.PointErc,
                PointName = p.PointName,
                KanalName = p.Kanal?.KanalName,
                DistributorName = p.Distributor?.DistributorName,
                PointGroupTypeName = p.PointGroupType?.PointGroupTypeName,
                AppUserFullName = $"{p.AppUser?.FirstName} {p.AppUser?.LastName}"
            }).ToList();

            return Ok(new
            {
                Data = dtoList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpGet("paged")]
        [Authorize(Roles = "Admin")] // Sadece Adminler tüm noktaları görebilir
        public IActionResult GetAllPointsPaged(int page = 1, int pageSize = 10)
        {
            var allPoints = _pointService.TGetListWithIncludes().AsQueryable();

            var totalCount = allPoints.Count();
            var pagedData = allPoints
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = pagedData.Select(p => new ResultPointDto
            {
                PointId = p.PointId,
                PointErc = p.PointErc,
                PointName = p.PointName,
                KanalName = p.Kanal?.KanalName,
                DistributorName = p.Distributor?.DistributorName,
                PointGroupTypeName = p.PointGroupType?.PointGroupTypeName,
                AppUserFullName = $"{p.AppUser?.FirstName} {p.AppUser?.LastName}"
            }).ToList();

            return Ok(new
            {
                Data = dtoList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        //Mevcut Metotların Güncellenmesi

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı ID'si alınamadı.");

            Point value;

            // Adminse her noktayı görebilir, değilse sadece kendi noktasını
            if (IsAdmin())
            {
                value = _pointService.TGetDetailsById(id); // TGetById yerine detayları çeken bir metodunuz varsa onu kullanın
            }
            else
            {
                value = _pointService.TGetByFilter(p => p.PointId == id && p.AppUserId == userId);
            }

            if (value == null)
                return NotFound("Nokta bulunamadı veya bu noktaya erişim yetkiniz yok.");

            // ResultPointDto'ya dönüşümde ilişkili verileri de dahil etmek için Select kullanıldı
            var dto = new ResultPointDto
            {
                PointId = value.PointId,
                PointErc = value.PointErc,
                PointName = value.PointName,
                KanalName = value.Kanal?.KanalName,
                DistributorName = value.Distributor?.DistributorName,
                PointGroupTypeName = value.PointGroupType?.PointGroupTypeName,
                AppUserFullName = $"{value.AppUser?.FirstName} {value.AppUser?.LastName}"
            };

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı ID'si alınamadı.");

            Point pointToDelete;

            // Admin ise her noktayı silebilir, değilse sadece kendi noktasını
            if (IsAdmin())
            {
                pointToDelete = _pointService.TGetById(id);
            }
            else
            {
                pointToDelete = _pointService.TGetByFilter(p => p.PointId == id && p.AppUserId == userId);
            }

            if (pointToDelete == null)
                return NotFound("Nokta bulunamadı veya bu noktayı silme yetkiniz yok.");

            _pointService.TDelete(id);
            return Ok("Nokta başarıyla silindi.");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreatePointDto createPointDto)
        {
            if (createPointDto == null)
                return BadRequest("Veri eksik.");

            var newPoint = _mapper.Map<Point>(createPointDto);

            var kanal = _kanalService.TGetById(createPointDto.KanalId);
            if (kanal == null)
                return BadRequest("Geçersiz KanalId.");

            // Kanal adı "DIST" ise AppUserId, Distributor'dan alınır.
            if (kanal.KanalName == "DIST")
            {
                if (!createPointDto.DistributorId.HasValue)
                    return BadRequest("DIST kanalı için DistributorId zorunludur.");

                var distributor = _distributorService.TGetById(createPointDto.DistributorId.Value);
                if (distributor == null)
                    return NotFound("Distributor bulunamadı.");

                newPoint.AppUserId = distributor.AppUserId;
            }
            else // "LC" veya "NA" gibi diğer kanallar için AppUserId doğrudan DTO'dan alınır.
            {
                // AppUserId gönderilmediyse veya geçersizse hata döndür
                if (createPointDto.AppUserId == 0)
                {
                    // Admin değilse, AppUserId'yi token'dan al.
                    // Adminler başkası adına nokta oluşturabilirken, diğerleri sadece kendi adlarına oluşturmalı.
                    if (!IsAdmin())
                    {
                        int authenticatedUserId = GetUserId();
                        if (authenticatedUserId == 0)
                            return Unauthorized("Kullanıcı ID'si token'dan alınamadı.");
                        newPoint.AppUserId = authenticatedUserId;
                    }
                    else
                    {
                        // Admin ise ve AppUserId belirtilmemişse BadRequest dön,
                        // çünkü Admin'in kimin adına nokta oluşturduğunu belirtmesi gerekir.
                        return BadRequest("Admin rolü için, LC/NA kanalları için AppUserId belirtilmelidir.");
                    }
                }
                else
                {
                    // Eğer AppUserId DTO'da belirtilmişse ve Admin değilse, token'daki ID ile eşleştiğinden emin ol.
                    if (!IsAdmin() && createPointDto.AppUserId != GetUserId())
                    {
                        return Forbid("Kendi AppUserId'niz dışındaki bir ID ile nokta oluşturmaya yetkiniz yok.");
                    }
                    newPoint.AppUserId = createPointDto.AppUserId;
                }
            }

            _pointService.TCreate(newPoint);
            return Ok("Nokta başarıyla oluşturuldu.");
        }

        [HttpPut]
        [Authorize]
        public IActionResult Update(UpdatePointDto updatePointDto)
        {
            int userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Kullanıcı ID'si alınamadı.");

            Point existingPoint;

            // Admin ise her noktayı güncelleyebilir, değilse sadece kendi noktasını
            if (IsAdmin())
            {
                existingPoint = _pointService.TGetById(updatePointDto.PointId);
            }
            else
            {
                existingPoint = _pointService.TGetByFilter(p => p.PointId == updatePointDto.PointId && p.AppUserId == userId);
            }

            if (existingPoint == null)
                return NotFound("Nokta bulunamadı veya bu noktayı güncelleme yetkiniz yok.");

            _mapper.Map(updatePointDto, existingPoint); // Mevcut nesneyi güncelle

            // AppUserId'yi değiştirmeyi engelle veya dikkatli ol
            // Eğer Admin değilse, AppUserId'nin değişmediğinden emin olmalıyız
            if (!IsAdmin())
            {
                existingPoint.AppUserId = userId; // Kendi ID'sini koru
            }

            _pointService.TUpdate(existingPoint);
            return Ok("Nokta başarıyla güncellendi.");
        }

        [HttpGet("count")] // Route adını "GetPointCount" yerine "count" olarak değiştirdim, daha RESTful
        [Authorize]
        public IActionResult GetPointCount()
        {
            int count;
            if (IsAdmin())
            {
                // Admin tüm noktaların sayısını görebilir
                count = _pointService.TCount();
            }
            else
            {
                // Diğer kullanıcılar sadece kendi noktalarının sayısını görebilir
                int userId = GetUserId();
                if (userId == 0)
                    return Unauthorized("Kullanıcı ID'si alınamadı.");
                count = _pointService.TFilteredCount(p => p.AppUserId == userId);
            }
            return Ok(count);
        }

        // Mevcut Diğer Metotlar
        // Bu metotların işleyişi değişmedi, ancak kullanıcının kendi yetkileri dahilinde çalışması için
        // servis katmanında filtrelenmeleri gerekebilir. Burada sadece KanalId ile getiriliyor,
        // bu metodu çağıranın Admin mi Bölge Müdürü mü olduğuna göre backend'de ek kontrol gerekebilir.
        [HttpGet("by-kanal/{kanalId}")]
        [Authorize]
        public IActionResult GetByKanal(int kanalId)
        {
            var list = _pointService.TGetByKanalId(kanalId);

            // Eğer kullanıcı Admin değilse, sadece kendi erişebileceği noktaları filtrele
            if (!IsAdmin())
            {
                int userId = GetUserId();
                list = list.Where(p => p.AppUserId == userId).ToList();
            }

            var result = list.Select(p => new ResultPointDto
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
        }

        [HttpGet("dropdown")]
        [Authorize]
        public IActionResult GetDropdown()
        {
            var list = _pointService.TGetList();

            // Eğer kullanıcı Admin değilse, sadece kendi erişebileceği noktaları filtrele
            if (!IsAdmin())
            {
                int userId = GetUserId();
                list = list.Where(p => p.AppUserId == userId).ToList();
            }

            var result = list
                .Select(p => new PointDropdownDto
                {
                    PointId = p.PointId,
                    PointName = p.PointName
                }).ToList();

            return Ok(result);
        }

        [HttpGet("by-group/{pointGroupTypeId}/distributors/{distributorId}")]
        [Authorize]
        public IActionResult GetByGroupAndDistributor(int distributorId, int pointGroupTypeId)
        {
            var list = _pointService.TGetByDistributorAndGroup(distributorId, pointGroupTypeId);

            // Eğer kullanıcı Admin değilse, sadece kendi erişebileceği noktaları filtrele
            if (!IsAdmin())
            {
                int userId = GetUserId();
                list = list.Where(p => p.AppUserId == userId).ToList();
            }

            var result = list.Select(p => new ResultPointDto
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
        }
    }
}




