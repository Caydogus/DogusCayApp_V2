using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin,BolgeMuduru")]
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly ITalepFormService _talepFormService;

    public AnalyticsController(ITalepFormService talepFormService)
    {
        _talepFormService = talepFormService;
    }

    // 1. Bölge müdürüne göre toplam talep sayısı
    [HttpGet("bolge-muduru-talep-sayisi")]
    public IActionResult BolgeMuduruTalepSayisi()
    {
        var data = _talepFormService
            .TGetAllWithUser()
            .GroupBy(t => t.AppUser.UserName)
            .Select(g => new {
                BolgeMuduru = g.Key,
                TalepSayisi = g.Count()
            })
            .OrderByDescending(x => x.TalepSayisi)
            .ToList();

        return Ok(data);
    }


    // 2. Her ürün için toplam talep edilen ürün adedi (Quantity)
    [HttpGet("top-talep-urun-adetleri")]
    public IActionResult TopTalepUrunAdetleri()
    {
        var data = _talepFormService
            .TGetAllWithUser()
            .GroupBy(t => t.ProductName)
            .Select(g => new {
                Urun = g.Key,
                ToplamAdet = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.ToplamAdet)
            .Take(10)
            .ToList();

        return Ok(data);
    }
}
