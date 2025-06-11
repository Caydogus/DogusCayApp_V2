using Microsoft.AspNetCore.Http; // HttpContext ve Session için using eklendi
using System.Security.Claims; // ClaimTypes için zaten vardı

namespace DogusCay.WebUI.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // Değişken adı daha açıklayıcı yapıldı

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Token'ı Session'a kaydeden metot
        public void SetUserToken(string token)
        {
            // HttpContext'in null olmadığından emin olarak Session'a token'ı kaydet
            _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", token);
        }

        // Session'dan token'ı okuyan metot
        public string GetUserToken()
        {
            // HttpContext'in null olmadığından emin olarak Session'dan token'ı oku
            return _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        }

        // Session'dan token'ı silen metot (Logout için)
        public void ClearUserToken()
        {
            // HttpContext'in null olmadığından emin olarak Session'dan token'ı sil
            _httpContextAccessor.HttpContext?.Session.Remove("JwtToken");
        }

        // Aşağıdaki property'ler WebUI'nin kendi oturumundan (cookie'den) bilgileri almak için kullanılabilir.
        // API'ye gönderilecek token ile doğrudan ilgileri yoktur.
        public int GetUserId
        {
            get
            {
                var idStr = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(idStr, out int userId))
                {
                    return userId;
                }
                return 0; // Eğer ID bulunamazsa veya dönüştürülemezse varsayılan değer
            }
        }

        public string GetUserRole => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        public string GetUserNameSurname => _httpContextAccessor.HttpContext?.User.FindFirst("fullName")?.Value;
    }
}