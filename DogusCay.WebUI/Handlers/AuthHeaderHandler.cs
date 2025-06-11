using Microsoft.AspNetCore.Http;
using DogusCay.WebUI.Services.TokenServices; // ITokenService'in bulunduğu namespace

namespace DogusCay.WebUI.Handlers
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // HttpContext'e erişebiliyorsak (bir web isteği sırasında)
            if (_httpContextAccessor.HttpContext != null)
            {
                // ITokenService'inizden token'ı alın
                var token = _tokenService.GetUserToken();

                if (!string.IsNullOrEmpty(token))
                {
                    // Authorization başlığına Bearer token'ı ekle
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }
            // İsteği bir sonraki handler'a veya hedefe ilet
            return await base.SendAsync(request, cancellationToken);
        }
    }
}