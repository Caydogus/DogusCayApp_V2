using DogusCay.WebUI.DTOs.TalepDtos;

namespace DogusCay.WebUI.DTOs.IhaleAnlasmaDtos
{
    public class ResultIhaleAnlasmaDto
    {
        public int AppUserId { get; set; }
        public string BolgeMuduru { get; set; }
        public string DistKod { get; set; }
        public string DistAdi { get; set; }
        public string NoktaKod { get; set; }
        public string NoktaAdi { get; set; }
        public bool SozlesmeYuklendi { get; set; }
        public decimal? IskontoOrani { get; set; }
        public TalepDurumu? TalepDurumu { get; set; }
        public DateTime? SozlesmeCreateDate { get; set; }
        public int? DosyaSayisi { get; set; }
        public int? SozlesmeId { get; set; }
    }
}
