using DogusCay.WebUI.DTOs.ChartDtos;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Route("admin/[controller]/[action]")]
public class ChartController : Controller
{
    private readonly HttpClient _client;
    public ChartController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("EduClient");
    }

    public async Task<IActionResult> Index()
    {
        var jwtToken = HttpContext.Session.GetString("JwtToken");
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

        // 1. Bölge müdürü verisi
        var bolgeData = await _client.GetFromJsonAsync<List<BolgeTalepAnalizDto>>("analytics/bolge-muduru-talep-sayisi");
        ViewBag.BolgeLabels = Newtonsoft.Json.JsonConvert.SerializeObject(bolgeData.Select(x => x.BolgeMuduru));
        ViewBag.BolgeValues = Newtonsoft.Json.JsonConvert.SerializeObject(bolgeData.Select(x => x.TalepSayisi));

        // 2. Ürünlere göre toplam talep edilen ürün adedi (Quantity)
        var urunAdetData = await _client.GetFromJsonAsync<List<UrunAdetAnalizDto>>("analytics/top-talep-urun-adetleri");
        ViewBag.UrunAdetLabels = Newtonsoft.Json.JsonConvert.SerializeObject(urunAdetData.Select(x => x.Urun));
        ViewBag.UrunAdetValues = Newtonsoft.Json.JsonConvert.SerializeObject(urunAdetData.Select(x => x.ToplamAdet));

        return View();
    }
}
