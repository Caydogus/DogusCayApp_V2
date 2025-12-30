

using DogusCay.DTO.DTOs.Attributes;

namespace DogusCay.DTO.DTOs.ExcelDtos
{
    public class ExportTalepFormDto
    {
        public int TalepFormId { get; set; }

        [ExcelIgnore]
        public int AppUserId { get; set; }
        public string AppUserName { get; set; }

        [ExcelIgnore]//bu kolon excelede aktarılmasın
        public string TalepTip { get; set; }

        [ExcelIgnore]
        public int KanalId { get; set; }
        public string KanalName { get; set; }

        [ExcelIgnore]
        public int? DistributorId { get; set; }
        public string DistributorName { get; set; }

        [ExcelIgnore]
        public int? PointGroupTypeId { get; set; }
        public string PointGroupTypeName { get; set; }

        [ExcelIgnore]
        public int PointId { get; set; }
        public string PointName { get; set; }

        [ExcelIgnore]
        public int CategoryId { get; set; }

        [ExcelIgnore]
        public string CategoryName { get; set; }

        [ExcelIgnore]
        public int? SubCategoryId { get; set; }

       
        public string SubCategoryName { get; set; }

        [ExcelIgnore]
        public int? SubSubCategoryId { get; set; }
        public string SubSubCategoryName { get; set; }


        [ExcelIgnore]
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

        [ExcelIgnore]
        public decimal? SabitBedelTL { get; set; }

        [ExcelIgnore]
        public decimal? Iskonto1 { get; set; }

        [ExcelIgnore]
        public decimal? Iskonto2 { get; set; }

        [ExcelIgnore]
        public decimal? Iskonto3 { get; set; }

        [ExcelIgnore]
        public decimal? Iskonto4 { get; set; }
        public string Note { get; set; }
        public decimal KoliToplamAgirligiKg { get; set; }
        public int KoliIciToplamAdet { get; set; }
        public decimal ListeFiyat { get; set; }
        public decimal SonAdetFiyati { get; set; }

        [ExcelIgnore]
        public decimal AdetFarkDonusuTL { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string TalepDurumu { get; set; }

        [ExcelIgnore]
        public int? OnaylayanAdminId { get; set; }
        public string OnaylayanAdminName { get; set; }

        [ExcelIgnore]
        public int? KampanyaDonusAdedi { get; set; }

        [ExcelIgnore]
        public string KampanyaResimYolu { get; set; }
        public decimal? AksiyonSatisFiyati { get; set; }
        public string? AksiyonTipi { get; set; } //22.10.2025 eklendi. 
        public string? IndirimTipi { get; set; } //22.10.2025 eklendi.

    }

}
