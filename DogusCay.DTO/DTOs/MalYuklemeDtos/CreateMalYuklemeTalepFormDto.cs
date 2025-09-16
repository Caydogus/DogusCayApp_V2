namespace DogusCay.DTO.DTOs.MalYuklemeDtos
{
    public class CreateMalYuklemeTalepFormDto
    {
       // public int AppUserId { get; set; }
        public int KanalId { get; set; }
        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; }
        public int PointId { get; set; }
        public decimal? Total { get; set; }              // Net toplam (iskontosuz bile olsa)
        public decimal? BrutTotal { get; set; }          // Brüt toplam
        public decimal? ToplamAgirlikKg { get; set; }    // Tüm ürünlerin toplam ağırlığı
        public decimal? Maliyet { get; set; }            // (Brüt - Net) / Brüt × 100
        public string? Note { get; set; }   // 12.09.2025
        public DateTime CreateDate { get; set; } //tarih otomatik eklenecek

        public List<CreateMalYuklemeTalepFormDetailDto>? MalYuklemeTalepFormDetails { get; set; }

    }

}
    