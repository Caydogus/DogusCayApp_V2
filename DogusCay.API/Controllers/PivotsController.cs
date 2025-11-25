
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PivotDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusCay.API.Controllers
{
    [Authorize]
    [Route("api/pivots")]
    [ApiController]
    public class PivotsController : ControllerBase
    {
        private readonly IPivotService _pivotService;

        public PivotsController(IPivotService pivotService)
        {
            _pivotService = pivotService;
        }

        // JWT'den AppUserId çekme
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // JWT'den rol çekme
        private string GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetPivotData([FromBody] PivotRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TableName))
                return BadRequest("Geçersiz istek. TableName zorunludur.");

            string userRole = GetUserRole();
            string userId = GetUserId();

            // Güvenlik kontrolü: userId boşsa token geçersiz
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı kimliği alınamadı.");

            var data = await _pivotService.TGetPivotDataAsync(request, userRole, userId);

            return Ok(data);
        }
    }
}
