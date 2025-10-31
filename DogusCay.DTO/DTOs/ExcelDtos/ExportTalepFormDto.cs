

namespace DogusCay.DTO.DTOs.ExcelDtos
{
    public class ExportTalepFormDto
    {
        public int TalepFormId { get; set; }

        public int AppUserId { get; set; }
        public string AppUserName { get; set; }

        public string TalepTip { get; set; }

        public int KanalId { get; set; }
        public string KanalName { get; set; }

        public int? DistributorId { get; set; }
        public string DistributorName { get; set; }

        public int? PointGroupTypeId { get; set; }
        public string PointGroupTypeName { get; set; }

        public int PointId { get; set; }
        public string PointName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int? SubSubCategoryId { get; set; }
        public string SubSubCategoryName { get; set; }

        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ErpCode { get; set; }

        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? OneriRafFiyati { get; set; }
        public decimal? OneriAksiyonFiyati { get; set; }
        public int? KoliIciAdet { get; set; }
        public decimal Total { get; set; }
        public decimal BrutTotal { get; set; }
        public decimal Maliyet { get; set; }
        public decimal? ApproximateWeightKg { get; set; }
        public decimal? SabitBedelTL { get; set; }
        public decimal? Iskonto1 { get; set; }
        public decimal? Iskonto2 { get; set; }
        public decimal? Iskonto3 { get; set; }
        public decimal? Iskonto4 { get; set; }
        public string Note { get; set; }
        public decimal KoliToplamAgirligiKg { get; set; }
        public int KoliIciToplamAdet { get; set; }
        public decimal ListeFiyat { get; set; }
        public decimal SonAdetFiyati { get; set; }
        public decimal AdetFarkDonusuTL { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string TalepDurumu { get; set; }
        public int? OnaylayanAdminId { get; set; }
        public string OnaylayanAdminName { get; set; }
        public int? KampanyaDonusAdedi { get; set; }
        public string KampanyaResimYolu { get; set; }
        public decimal? AksiyonSatisFiyati { get; set; }
        public string? AksiyonTipi { get; set; } //22.10.2025 eklendi. 
        public string? IndirimTipi { get; set; } //22.10.2025 eklendi.

    }

}
