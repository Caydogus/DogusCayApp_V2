using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.KanalDtos;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.DTO.DTOs.UserDtos;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.DTO.DTOs.MalYuklemeDtos
{
    public class ResultMalYuklemeTalepFormDto
    {
        public int MalYuklemeTalepFormId { get; set; }
        public int AppUserId { get; set; }
        public int KanalId { get; set; }
        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; }
        public int PointId { get; set; }
        public string? AppUserName { get; set; }
        public string? KanalName { get; set; }
        public string? PointName { get; set; }
        public DateTime CreateDate { get; set; }

        public decimal? Total { get; set; }              // Net toplam (iskontosuz bile olsa)
        public decimal? BrutTotal { get; set; }          // Brüt toplam
        public decimal? ToplamAgirlikKg { get; set; }    // Tüm ürünlerin toplam ağırlığı
        public decimal? Maliyet { get; set; }            // (Brüt - Net) / Brüt × 100
        public TalepDurumu TalepDurumu { get; set; }
        public ResultKanalDto? Kanal { get; set; }
        public ResultPointDto? Point { get; set; }
        public ResultUserDto? AppUser { get; set; }
        public List<ResultMalYuklemeTalepFormDetailDto>? MalYuklemeTalepFormDetails { get; set; }
    }
}
