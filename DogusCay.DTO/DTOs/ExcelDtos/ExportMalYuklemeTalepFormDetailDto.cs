using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.ExcelDtos
{
    public class ExportMalYuklemeTalepFormDetailDto
    {
        public int MalYuklemeTalepFormDetailId { get; set; }
        public int MalYuklemeTalepFormId { get; set; } // eşleştirme için

        // Kategori zinciri
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public int? SubSubCategoryId { get; set; }
        public string? SubSubCategoryName { get; set; }

        // Ürün bilgileri
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ErpCode { get; set; }
        public int UnitTypeId { get; set; }
        public decimal ApproximateWeightKg { get; set; }

        // Fiyat & adetler
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public int Quantity { get; set; }

        // İndirimler & hesaplamalar
        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? FixedPrice { get; set; }
        public decimal? NetTutar { get; set; }
        public decimal? NetAdetFiyat { get; set; }
        public decimal? BrutTutar { get; set; }
        public decimal? Maliyet { get; set; }
    }
}
