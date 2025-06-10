namespace DogusCay.WebUI.DTOs.TalepDtos
{
    public class ResultTalepFormDto
    {
        // Zincir alanları (isim bazlı)
        public int TalepFormId { get; set; }
        public string KanalName { get; set; }
        public string DistributorName { get; set; }
        public string PointGroupTypeName { get; set; }
        public string PointName { get; set; }

        // Kategori bilgileri (isim bazlı)
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubSubCategoryName { get; set; }
        public string UserFullName { get; set; }
        // Ürün bilgileri
        public string ProductName { get; set; }
        public string ErpCode { get; set; } // opsiyonel
        public decimal ApproximateWeightKg { get; set; }
        public int KoliIciAdet { get; set; }

        // Talep detayları
        public int Quantity { get; set; }
        public decimal Price { get; set; }           // Ürün birim fiyatı
        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }

        public decimal AdetFarkDonusuTL { get; set; }
        public decimal? SabitBedelTL { get; set; }

        // Hesaplananlar
        public decimal ListeFiyat { get; set; }
        public decimal SonAdetFiyati { get; set; }
        public int KoliIciToplamAdet { get; set; }
        public decimal KoliToplamAgirligiKg { get; set; }
        public decimal Total { get; set; }

        // Tarihler
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public string Note { get; set; }
    }
}
