using DogusCay.Entity.Entities.Talep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.IhaleAnlasmaDtos
{
    public class ResultIhaleAnlasmaSozlesmeDto
    {
        public int IhaleAnlasmaSozlesmeId { get; set; }
        public int IhaleAnlasmaId { get; set; }
        public string BolgeMuduru { get; set; }
        public string DistKod { get; set; }
        public string DistAdi { get; set; }
        public string NoktaKod { get; set; }
        public string NoktaAdi { get; set; }
        public decimal? IskontoOrani { get; set; }
        public DateTime CreateDate { get; set; }
        public TalepDurumu TalepDurumu { get; set; }
        public string? OnaylayanAdminName { get; set; }
        public string? Note { get; set; }

        // Dosyalar
        public List<ResultIhaleAnlasmaDosyaDto> Dosyalar { get; set; }
    }
}
