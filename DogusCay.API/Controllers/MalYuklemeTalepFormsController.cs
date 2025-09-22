using System.Security.Claims;
using AutoMapper;
using ClosedXML.Excel;
using DogusCay.API.Helpers;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.ExcelDtos;
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Authorize]
    [Route("api/malyuklemetalepforms")]
    [ApiController]
    public class MalYuklemeTalepFormsController : ControllerBase
    {
        private readonly IMalYuklemeTalepFormService _malYuklemeTalepFormService;
        private readonly IMapper _mapper;
        private readonly MailHelper _mailHelper;
        public MalYuklemeTalepFormsController(IMalYuklemeTalepFormService malYuklemeTalepFormService, IMapper mapper, MailHelper mailHelper)
        {
            _malYuklemeTalepFormService = malYuklemeTalepFormService;
            _mapper = mapper;
            _mailHelper = mailHelper;
        }
        private int GetUserId()
        {
            var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        [HttpPost]
        [Authorize(Roles = "BolgeMuduru")]
        public IActionResult Create([FromBody] CreateMalYuklemeTalepFormDto dto)
        {
            int authenticatedUserId = GetUserId();
            if (authenticatedUserId == 0)
                return Unauthorized("Kullanıcı kimliği alınamadı. Lütfen giriş yapın.");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                return BadRequest(new { Message = "Gönderilen veri geçerli değil.", Errors = errors });
            }

            try
            {
                var createdForm = _malYuklemeTalepFormService
                    .TCreateMalYuklemeTalepForm(dto, authenticatedUserId);

                //Navigation property’ler dolu şekilde tekrar çekiyoruz
                var fullForm = _malYuklemeTalepFormService
                    .TGetByIdWithUserAndPoint(createdForm.MalYuklemeTalepFormId);

                var userName = fullForm.AppUser != null
                    ? $"{fullForm.AppUser.FirstName} {fullForm.AppUser.LastName}"
                    : "Bilinmeyen Kullanıcı";

                var pointName = fullForm.Point != null
                    ? fullForm.Point.PointName
                    : "Bilinmeyen Müşteri";

                //Mail gönderimi opsiyonel ve asenkron
                Task.Run(() =>
                {
                    try
                    {
                        var notify = _mailHelper.GetNotifyAddress();
                        _mailHelper.Send(notify,
                            "Yeni Ürün Yükleme Talebi",
                            $"Yeni ürün yükleme talebi oluşturuldu.\n" +
                            $"Talep No: {fullForm.MalYuklemeTalepFormId}\n" +
                            $"Kullanıcı: {userName}\n" +
                            $"Müşteri (Nokta): {pointName}");
                    }
                    catch (Exception mailEx)
                    {
                        Console.WriteLine("📧 Ürün yükleme bilgilendirme maili gönderilemedi: " + mailEx.Message);
                        // TODO: ILogger/Serilog ile loglanabilir
                    }
                });

                return Ok(new
                {
                    success = true,
                    message = "Ürün Yükleme talebi başarıyla oluşturuldu.",
                    formId = createdForm.MalYuklemeTalepFormId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Form oluşturulurken beklenmeyen bir hata oluştu. " + ex.Message);
            }
        }


        [HttpGet("mine-paged")]
        [Authorize(Roles = "BolgeMuduru")]
        public IActionResult GetOwnFormsPaged(int page = 1, int pageSize = 7)
        {
            Console.WriteLine(" mine-paged endpoint çağrıldı");

            int userId = GetUserId();
            if (userId == 0)
            {
                Console.WriteLine("❌ Kullanıcı ID alınamadı (userId = 0)");
                return Unauthorized("Kullanıcı ID'si alınamadı.");
            }

            var userForms = _malYuklemeTalepFormService.TGetAllByUserId(userId).AsQueryable();
            Console.WriteLine($"👤 Kullanıcı ID: {userId}, Toplam kayıt: {userForms.Count()}");

            var totalCount = userForms.Count();
            var pagedData = userForms
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = _mapper.Map<List<ResultMalYuklemeTalepFormDto>>(pagedData);

            return Ok(new
            {
                Data = dtoList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }




        [HttpGet("paged")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllFormsPaged(int page = 1, int pageSize = 7)
        {
            var allForms = _malYuklemeTalepFormService.TGetAllWithUser().AsQueryable();

            var totalCount = allForms.Count();
            var pagedData = allForms
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = _mapper.Map<List<ResultMalYuklemeTalepFormDto>>(pagedData);

            return Ok(new
            {
                Data = dtoList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        // mal yukleme Talep formunu onaylar (Admin rolü için).
        [HttpPost("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            int adminId = GetUserId();
            if (adminId == 0)
                return Unauthorized("Admin ID'si token'dan alınamadı.");

            var form = _malYuklemeTalepFormService.TGetByIdWithUserAndPoint(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            _malYuklemeTalepFormService.TUpdateStatus(id, TalepDurumu.Onaylandi, adminId);

            //  Mail gönderimini asenkron yapıyoruz
            Task.Run(() =>
            {
                try
                {
                    var userEmail = form.AppUser?.Email;
                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        var userName = $"{form.AppUser.FirstName} {form.AppUser.LastName}";
                        var pointName = form.Point?.PointName ?? "Bilinmeyen Müşteri";

                        _mailHelper.Send(
                            userEmail,
                            "Ürün Yükleme Talebiniz Onaylandı",
                            $"Merhaba {userName},\n" +
                            $"{pointName} için açmış olduğunuz " +
                            $"{form.MalYuklemeTalepFormId} numaralı ürün yükleme talebi onaylandı."
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("📧 Onay maili gönderilemedi: " + ex.Message);
                    // TODO: ILogger veya Serilog ile logla
                }
            });

            return Ok("Talep onaylandı.");
        }

        // mal yukleme Talep formunu reddeder (Admin rolü için).
        [HttpPost("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            int adminId = GetUserId();
            if (adminId == 0)
                return Unauthorized("Admin ID'si token'dan alınamadı.");

            var form = _malYuklemeTalepFormService.TGetByIdWithUserAndPoint(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            _malYuklemeTalepFormService.TUpdateStatus(id, TalepDurumu.Reddedildi, adminId);

            //  Mail gönderimi opsiyonel ve asenkron
            Task.Run(() =>
            {
                try
                {
                    var userEmail = form.AppUser?.Email;
                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        var userName = $"{form.AppUser.FirstName} {form.AppUser.LastName}";
                        var pointName = form.Point?.PointName ?? "Bilinmeyen Müşteri";

                        _mailHelper.Send(
                            userEmail,
                            "Ürün Yükleme Talebiniz Reddedildi",
                            $"Merhaba {userName},\n" +
                            $"{pointName} için açmış olduğunuz " +
                            $"{form.MalYuklemeTalepFormId} numaralı ürün yükleme talebi reddedildi."
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("📧 Red maili gönderilemedi: " + ex.Message);
                    
                }
            });

            return Ok("Talep reddedildi.");
        }


        // Kullanıcının kendi onaylanmamış formunu silebilir.
        [Authorize(Roles = "Admin,BolgeMuduru")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var form = _malYuklemeTalepFormService.TGetById(id);
            if (form == null)
                return NotFound("Talep bulunamadı.");

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin) // Bölge Müdürü ise kontrol yap
            {
                if (form.AppUserId != currentUserId)
                    return Forbid("Yalnızca kendi taleplerinizi silebilirsiniz.");

                if (form.TalepDurumu != TalepDurumu.Bekliyor)
                    return BadRequest("Sadece 'Bekliyor' durumundaki talepleri silebilirsiniz.");
            }

            _malYuklemeTalepFormService.TDelete(id);
            return Ok("Talep başarıyla silindi.");
        }




        [HttpGet("{id}")] // Bu endpoint api/MalYuklemeTalepForms/{id} şeklindeki isteklere yanıt verecek
        [Authorize] // Giriş yapmış her kullanıcı kendi formuna erişebilir, Adminler tüm formlara
        public IActionResult GetDetails(int id)
        {
            try
            {
                var formEntity = _malYuklemeTalepFormService.TGetDetailsForForm(id); // Servis katmanından detayları çek
                if (formEntity == null)
                {
                    return NotFound($"ID'si {id} olan Mal Yükleme Talep Formu bulunamadı.");
                }

                // Entity'yi DTO'ya map'le
                var formDto = _mapper.Map<ResultMalYuklemeTalepFormDto>(formEntity);

                return Ok(formDto); // Detay DTO'sunu döndür
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"❌ API Hatası (GetDetails): Mal Yükleme Talep Formu detayları getirilirken beklenmeyen bir hata oluştu: {ex.Message}");
                return StatusCode(500, "Detaylar getirilirken beklenmeyen bir hata oluştu.");
            }
        }

        [HttpGet("export-excel")]
        public IActionResult ExportToExcel()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            List<ExportMalYuklemeTalepFormDto> forms;
            List<ExportMalYuklemeTalepFormDetailDto> details;

            if (role == "Admin")
            {
                forms = _malYuklemeTalepFormService.TGetAllForExport();
                details = _malYuklemeTalepFormService.TGetAllDetailsForExport();
            }
            else
            {
                forms = _malYuklemeTalepFormService.TGetListForExportByUserId(userId);
                details = _malYuklemeTalepFormService.TGetDetailsForExportByUserId(userId);
            }

            // Hem formlar hem detaylar boşsa Excel indirme
            if ((forms == null || !forms.Any()) && (details == null || !details.Any()))
            {
                return NoContent();
            }

            using (var workbook = new XLWorkbook())
            {
                //1. Sayfa: Formlar
                if (forms != null && forms.Any())
                {
                    var wsForms = workbook.Worksheets.Add("Formlar");
                    var propsForms = typeof(ExportMalYuklemeTalepFormDto).GetProperties();

                    for (int i = 0; i < propsForms.Length; i++)
                        wsForms.Cell(1, i + 1).Value = propsForms[i].Name;

                    for (int row = 0; row < forms.Count; row++)
                    {
                        for (int col = 0; col < propsForms.Length; col++)
                        {
                            var value = propsForms[col].GetValue(forms[row]);
                            wsForms.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                        }
                    }
                }

                //2. Sayfa: Detaylar
                if (details != null && details.Any())
                {
                    var wsDetails = workbook.Worksheets.Add("Detaylar");
                    var propsDetails = typeof(ExportMalYuklemeTalepFormDetailDto).GetProperties();

                    for (int i = 0; i < propsDetails.Length; i++)
                        wsDetails.Cell(1, i + 1).Value = propsDetails[i].Name;

                    for (int row = 0; row < details.Count; row++)
                    {
                        for (int col = 0; col < propsDetails.Length; col++)
                        {
                            var value = propsDetails[col].GetValue(details[row]);
                            wsDetails.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "MalYuklemeTalepForms.xlsx");
                }
            }
        }


    }
}
