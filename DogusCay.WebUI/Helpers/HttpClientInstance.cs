using Microsoft.Extensions.Configuration;

namespace DogusCay.WebUI.Helpers
{
    public static class HttpClientInstance
    {
        public static HttpClient CreateClient(IConfiguration configuration)
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("ApiSettings:BaseUrl appsettings dosyasında tanımlı değil!");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            return client;
        }
    }
}
