using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.Attributes;
namespace DogusCay.DTO.DTOs.ExcelDtos
{
    public class ExportMalYuklemeTalepFormDto
    {
        public int MalYuklemeTalepFormId { get; set; }

        public int AppUserId { get; set; }
        public string AppUserName { get; set; }

        public string KanalName { get; set; }
        public string? DistributorName { get; set; }
        public string? PointGroupTypeName { get; set; }
        public string PointName { get; set; }

        public DateTime CreateDate { get; set; }

        [ExcelIgnore]
        public string TalepTip { get; set; }
        public string TalepDurumu { get; set; }

        [ExcelIgnore]
        public int? OnaylayanAdminId { get; set; }
        public string? OnaylayanAdminName { get; set; }

        // Finansal ve özet bilgiler
        public decimal? Total { get; set; }
        public decimal? BrutTotal { get; set; }
        public decimal? ToplamAgirlikKg { get; set; }
        public decimal? Maliyet { get; set; }
        public string? Note { get; set; }
    }
}
