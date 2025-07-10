using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.MalYuklemeTalepFormDtos
{
    public class ResultMalYuklemeTalepFormDetailDto
    {
        public int ResultMalYuklemeTalepFormDetailId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } // <<-- BURAYA EKLEYİN: Product için navigation property

        public string ProductName { get; set; }
        public string ErpCode { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public Category SubCategory { get; set; } // <<-- BURAYA EKLEYİN: Alt kategori için navigation property
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? SubSubCategoryName { get; set; }
        public int? SubSubCategoryId { get; set; }
        public int UnitTypeId { get; set; }
        public decimal ApproximateWeightKg { get; set; }
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount1 { get; set; }      // İskonto 1 (%)
        public decimal? Discount2 { get; set; }      // İskonto 2 (%)
        public decimal? FixedPrice { get; set; }     // Sabit Bedel
        public decimal? NetTutar { get; set; }       // Hesaplanan Net Tutar
        public decimal? NetAdetFiyat { get; set; }   // Hesaplanan Net Adet Fiyatı
        public decimal? BrutTutar { get; set; }      // Hesaplanan Brüt Tutar
        public decimal? Maliyet { get; set; }        // Hesaplanan Maliyet (%)
    }
}
