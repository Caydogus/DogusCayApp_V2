using DogusCay.WebUI.DTOs.DistributorDtos;
using DogusCay.WebUI.DTOs.KanalDtos;
using DogusCay.WebUI.DTOs.PointDtos;
using DogusCay.WebUI.DTOs.PointGrupDtos;
using DogusCay.WebUI.DTOs.TalepDtos;
using DogusCay.WebUI.DTOs.UserDtos;

namespace DogusCay.WebUI.DTOs.MalYuklemeTalepFormDtos
{
    public class ResultMalYuklemeTalepFormDto
    {
        public int MalYuklemeTalepFormId { get; set; }

        public int AppUserId { get; set; }
        public ResultUserDto? AppUser { get; set; }

        public int KanalId { get; set; }
        public ResultKanalDto? Kanal { get; set; }

        public int? DistributorId { get; set; }
        public ResultDistributorDto? Distributor { get; set; }

        public int? PointGroupTypeId { get; set; }
        public ResultPointGroupTypeDto? PointGroupType { get; set; }

        public int PointId { get; set; }
        public ResultPointDto? Point { get; set; }

        public decimal? Total { get; set; }              // Net toplam (iskontosuz bile olsa)
        public decimal? BrutTotal { get; set; }          // Brüt toplam
        public decimal? ToplamAgirlikKg { get; set; }    // Tüm ürünlerin toplam ağırlığı
        public decimal? Maliyet { get; set; }            // (Brüt - Net) / Brüt × 100
        public TalepDurumu TalepDurumu { get; set; }
        public string? Note { get; set; }   // 12.09.2025
        public DateTime CreateDate { get; set; } //tarih otomatik eklenecek

        public List<ResultMalYuklemeTalepFormDetailDto>? MalYuklemeTalepFormDetails { get; set; }
    }
}
