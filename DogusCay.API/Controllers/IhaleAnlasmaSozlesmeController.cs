using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.IhaleAnlasmaDtos;
using DogusCay.Entity.Entities.IhaleAnlasma;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{
    [Authorize]
    [Route("api/ihaleanlasmalar")]
    [ApiController]
    public class IhaleAnlasmaSozlesmeController : ControllerBase
    {
        private readonly IIhaleAnlasmaSozlesmeService _service;
        private readonly string _uploadRootPath;

        public IhaleAnlasmaSozlesmeController(
            IIhaleAnlasmaSozlesmeService service,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _service = service;

            var configuredPath = configuration["UploadSettings:WebUIRootPath"];

            if (!string.IsNullOrEmpty(configuredPath))
            {
                if (Path.IsPathRooted(configuredPath))
                    _uploadRootPath = configuredPath;
                else
                    _uploadRootPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, configuredPath));
            }
            else
            {
                // fallback (API wwwroot)
                _uploadRootPath = env.WebRootPath;
            }
        }

        private int GetUserId()
        {
            var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        // ============================================================
        // LİSTELEME
        // ============================================================

        // Bölge Müdürü - Kendi ihale anlaşma listesi
        [HttpGet("my-list")]
        [Authorize(Roles = "BolgeMuduru")]
        public IActionResult GetMyAnlasmaList()
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized("Kullanıcı kimliği alınamadı.");

            var anlasmaList = _service.TGetAnlasmaListByUserId(userId);
            var sozlesmeList = _service.TGetAllByUserId(userId);
            var sozlesmeMap = sozlesmeList.ToDictionary(s => s.NoktaKod);

            var result = anlasmaList.Select(x =>
            {
                sozlesmeMap.TryGetValue(x.NoktaKod, out var sozlesme);
                return new ResultIhaleAnlasmaDto
                {
                    AppUserId = x.AppUserId,
                    BolgeMuduru = x.BolgeMuduru,
                    DistKod = x.DistKod,
                    DistAdi = x.DistAdi,
                    NoktaKod = x.NoktaKod,
                    NoktaAdi = x.NoktaAdi,
                    SozlesmeYuklendi = sozlesme != null,
                    IskontoOrani = sozlesme?.IskontoOrani,
                    TalepDurumu = sozlesme?.TalepDurumu,
                    SozlesmeCreateDate = sozlesme?.CreateDate,
                    DosyaSayisi = sozlesme?.Dosyalar?.Count ?? 0,
                    SozlesmeId = sozlesme?.IhaleAnlasmaSozlesmeId
                };
            }).ToList();

            return Ok(result);
        }

        // Admin - Tüm sözleşme listesi
        [HttpGet("all-list")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllAnlasmaList()
        {
            var sozlesmeList = _service.TGetAllForAdmin();

            var result = sozlesmeList.Select(s =>
            {
                var anlasma = _service.TGetAnlasmaByNoktaKod(s.NoktaKod);
                return new ResultIhaleAnlasmaDto
                {
                    AppUserId = s.AppUserId,
                    NoktaKod = s.NoktaKod,
                    BolgeMuduru = anlasma?.BolgeMuduru,
                    DistKod = anlasma?.DistKod,
                    DistAdi = anlasma?.DistAdi,
                    NoktaAdi = anlasma?.NoktaAdi,
                    IskontoOrani = s.IskontoOrani,
                    TalepDurumu = s.TalepDurumu,
                    SozlesmeCreateDate = s.CreateDate,
                    DosyaSayisi = s.Dosyalar?.Count ?? 0,
                    SozlesmeYuklendi = true,
                    SozlesmeId = s.IhaleAnlasmaSozlesmeId
                };
            }).ToList();

            return Ok(result);
        }

        // Admin - Tüm noktalar (linked server + sözleşme durumu)
        [HttpGet("all-noktalar")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllNoktalar()
        {
            var allAnlasma = _service.TGetAllAnlasma();
            var sozlesmeList = _service.TGetAllForAdmin();
            var sozlesmeMap = sozlesmeList.ToDictionary(s => s.NoktaKod);

            var result = allAnlasma.Select(x =>
            {
                sozlesmeMap.TryGetValue(x.NoktaKod, out var sozlesme);
                return new ResultIhaleAnlasmaDto
                {
                    AppUserId = x.AppUserId,
                    BolgeMuduru = x.BolgeMuduru,
                    DistKod = x.DistKod,
                    DistAdi = x.DistAdi,
                    NoktaKod = x.NoktaKod,
                    NoktaAdi = x.NoktaAdi,
                    SozlesmeYuklendi = sozlesme != null,
                    IskontoOrani = sozlesme?.IskontoOrani,
                    TalepDurumu = sozlesme?.TalepDurumu,
                    SozlesmeCreateDate = sozlesme?.CreateDate,
                    DosyaSayisi = sozlesme?.Dosyalar?.Count ?? 0,
                    SozlesmeId = sozlesme?.IhaleAnlasmaSozlesmeId
                };
            }).ToList();

            return Ok(result);
        }

        // ============================================================
        // DETAY
        // ============================================================

        [HttpGet("sozlesme/{id}")]
        public IActionResult GetSozlesmeDetail(int id)
        {
            var sozlesme = _service.TGetDetailsById(id);
            if (sozlesme == null) return NotFound("Sözleşme bulunamadı.");

            var anlasma = _service.TGetAnlasmaByNoktaKod(sozlesme.NoktaKod);

            var dto = new ResultIhaleAnlasmaSozlesmeDto
            {
                IhaleAnlasmaSozlesmeId = sozlesme.IhaleAnlasmaSozlesmeId,
                NoktaKod = sozlesme.NoktaKod,
                BolgeMuduru = anlasma?.BolgeMuduru,
                DistKod = anlasma?.DistKod,
                DistAdi = anlasma?.DistAdi,
                NoktaAdi = anlasma?.NoktaAdi,
                IskontoOrani = sozlesme.IskontoOrani,
                CreateDate = sozlesme.CreateDate,
                TalepDurumu = sozlesme.TalepDurumu,
                OnaylayanAdminName = sozlesme.OnaylayanAdmin != null
                    ? $"{sozlesme.OnaylayanAdmin.FirstName} {sozlesme.OnaylayanAdmin.LastName}"
                    : null,
                Note = sozlesme.Note,
                Dosyalar = sozlesme.Dosyalar?.Select(d => new ResultIhaleAnlasmaDosyaDto
                {
                    IhaleAnlasmaDosyaId = d.IhaleAnlasmaDosyaId,
                    DosyaAdi = d.DosyaAdi,
                    DosyaYolu = d.DosyaYolu,
                    DosyaTipi = d.DosyaTipi,
                    DosyaBoyutu = d.DosyaBoyutu,
                    SayfaSirasi = d.SayfaSirasi
                }).ToList()
            };

            return Ok(dto);
        }

        // ============================================================
        // CREATE
        // ============================================================

        [HttpPost("sozlesme")]
        [Authorize(Roles = "BolgeMuduru")]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)]
        public async Task<IActionResult> CreateSozlesme(
            [FromForm] CreateIhaleAnlasmaSozlesmeDto dto,
            [FromForm] List<IFormFile> dosyalar)
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized("Kullanıcı kimliği alınamadı.");

            if (dosyalar == null || !dosyalar.Any())
                return BadRequest("En az bir dosya yüklenmelidir.");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "application/pdf" };
            const long maxFileSize = 50 * 1024 * 1024;

            foreach (var file in dosyalar)
            {
                if (!allowedTypes.Contains(file.ContentType.ToLower()))
                    return BadRequest($"Desteklenmeyen dosya tipi: {file.FileName}. Sadece JPG, PNG ve PDF kabul edilir.");
                if (file.Length > maxFileSize)
                    return BadRequest($"Dosya çok büyük: {file.FileName}. Maksimum 50MB.");
            }

            try
            {
                var safeNoktaKod = Path.GetFileName(dto.NoktaKod);
                var uploadFolder = Path.Combine(_uploadRootPath, "uploads", "ihale-sozlesme", safeNoktaKod);
                Directory.CreateDirectory(uploadFolder);

                var dosyaEntityList = new List<IhaleAnlasmaDosya>();
                int sira = 1;

                foreach (var file in dosyalar)
                {
                    var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadFolder, uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    dosyaEntityList.Add(new IhaleAnlasmaDosya
                    {
                        DosyaAdi = file.FileName,
                        DosyaYolu = $"/uploads/ihale-sozlesme/{safeNoktaKod}/{uniqueName}",
                        DosyaTipi = file.ContentType,
                        DosyaBoyutu = file.Length,
                        SayfaSirasi = sira++,
                        YuklenmeTarihi = DateTime.Now
                    });
                }

                var sozlesme = _service.TCreateSozlesme(dto, userId, dosyaEntityList);

                return Ok(new
                {
                    success = true,
                    message = "Sözleşme başarıyla yüklendi.",
                    sozlesmeId = sozlesme.IhaleAnlasmaSozlesmeId
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Sözleşme yüklenirken bir hata oluştu: " + ex.Message);
            }
        }

        // ============================================================
        // UPDATE
        // ============================================================

        [HttpPut("sozlesme/{id}")]
        [Authorize(Roles = "BolgeMuduru")]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)]
        public async Task<IActionResult> UpdateSozlesme(int id,
            [FromForm] decimal iskontoOrani,
            [FromForm] string? note,
            [FromForm] List<IFormFile>? yeniDosyalar)
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized("Kullanıcı kimliği alınamadı.");

            var sozlesme = _service.TGetDetailsById(id);
            if (sozlesme == null) return NotFound("Sözleşme bulunamadı.");

            if (sozlesme.AppUserId != userId)
                return Forbid();

            if (sozlesme.TalepDurumu == TalepDurumu.Onaylandi)
                return BadRequest("Onaylanmış sözleşmeler güncellenemez.");

            sozlesme.IskontoOrani = iskontoOrani;
            sozlesme.Note = note;
            sozlesme.TalepDurumu = TalepDurumu.Bekliyor;

            if (yeniDosyalar != null && yeniDosyalar.Any())
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "application/pdf" };
                const long maxFileSize = 50 * 1024 * 1024;

                foreach (var file in yeniDosyalar)
                {
                    if (!allowedTypes.Contains(file.ContentType.ToLower()))
                        return BadRequest($"Desteklenmeyen dosya tipi: {file.FileName}");
                    if (file.Length > maxFileSize)
                        return BadRequest($"Dosya çok büyük: {file.FileName}. Maksimum 50MB.");
                }

                var uploadFolder = Path.Combine(_uploadRootPath, "uploads", "ihale-sozlesme", sozlesme.NoktaKod);
                Directory.CreateDirectory(uploadFolder);

                int maxSira = sozlesme.Dosyalar?.Any() == true ? sozlesme.Dosyalar.Max(d => d.SayfaSirasi) : 0;

                foreach (var file in yeniDosyalar)
                {
                    var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadFolder, uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    sozlesme.Dosyalar.Add(new IhaleAnlasmaDosya
                    {
                        DosyaAdi = file.FileName,
                        DosyaYolu = $"/uploads/ihale-sozlesme/{sozlesme.NoktaKod}/{uniqueName}",
                        DosyaTipi = file.ContentType,
                        DosyaBoyutu = file.Length,
                        SayfaSirasi = ++maxSira,
                        YuklenmeTarihi = DateTime.Now
                    });
                }
            }

            _service.TUpdate(sozlesme);

            return Ok(new { success = true, message = "Sözleşme güncellendi." });
        }

        // ============================================================
        // ONAY / RED
        // ============================================================

        [HttpPost("sozlesme/approve/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            int adminId = GetUserId();
            if (adminId == 0) return Unauthorized("Admin ID alınamadı.");

            var sozlesme = _service.TGetDetailsById(id);
            if (sozlesme == null) return NotFound("Sözleşme bulunamadı.");

            _service.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);
            return Ok("Sözleşme onaylandı.");
        }

        [HttpPost("sozlesme/reject/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            int adminId = GetUserId();
            if (adminId == 0) return Unauthorized("Admin ID alınamadı.");

            var sozlesme = _service.TGetDetailsById(id);
            if (sozlesme == null) return NotFound("Sözleşme bulunamadı.");

            _service.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);
            return Ok("Sözleşme reddedildi.");
        }

        // ============================================================
        // DOSYA SİLME
        // ============================================================

        [HttpDelete("dosya/{dosyaId}")]
        [Authorize(Roles = "BolgeMuduru")]
        public IActionResult DeleteDosya(int dosyaId)
        {
            int userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var dosya = _service.TGetDosyaById(dosyaId);
            if (dosya == null) return NotFound("Dosya bulunamadı.");

            if (dosya.IhaleAnlasmaSozlesme.AppUserId != userId)
                return Forbid();

            if (dosya.IhaleAnlasmaSozlesme.TalepDurumu == TalepDurumu.Onaylandi)
                return BadRequest("Onaylanmış sözleşmenin dosyaları silinemez.");

            var fullPath = Path.Combine(_uploadRootPath, dosya.DosyaYolu.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _service.TDeleteDosya(dosyaId);

            return Ok("Dosya silindi.");
        }

        // ============================================================
        // SÖZLEŞME SİLME
        // ============================================================

        [HttpDelete("sozlesme/{id}")]
        [Authorize(Roles = "Admin,BolgeMuduru")]
        public IActionResult Delete(int id)
        {
            var sozlesme = _service.TGetById(id);
            if (sozlesme == null) return NotFound("Sözleşme bulunamadı.");

            int currentUserId = GetUserId();
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                if (sozlesme.AppUserId != currentUserId)
                    return Forbid();
                if (sozlesme.TalepDurumu != TalepDurumu.Bekliyor)
                    return BadRequest("Sadece 'Bekliyor' durumundaki sözleşmeler silinebilir.");
            }

            var detayli = _service.TGetDetailsById(id);
            if (detayli?.Dosyalar != null)
            {
                foreach (var dosya in detayli.Dosyalar)
                {
                    var fullPath = Path.Combine(_uploadRootPath, dosya.DosyaYolu.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
            }

            _service.TDelete(id);
            return Ok("Sözleşme başarıyla silindi.");
        }
    }
}