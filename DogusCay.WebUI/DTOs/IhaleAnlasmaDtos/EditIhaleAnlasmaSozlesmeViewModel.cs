using System.ComponentModel.DataAnnotations;

namespace DogusCay.WebUI.DTOs.IhaleAnlasmaDtos
{
    public class EditIhaleAnlasmaSozlesmeViewModel
    {
        public int IhaleAnlasmaSozlesmeId { get; set; }

        public string NoktaKod { get; set; }
        public string? NoktaAdi { get; set; }

        [Required(ErrorMessage = "İskonto oranı zorunludur.")]
        [Range(0, 50, ErrorMessage = "İskonto oranı 0 ile 50 arasında olmalıdır.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Ondalıklı sayı girilemez.")]
        public decimal? IskontoOrani { get; set; }

        public string? Note { get; set; }

        // Yeni eklenecek dosyalar
        public List<IFormFile>? YeniDosyalar { get; set; }

        // Mevcut dosyalar (görüntüleme için)
        public List<ResultIhaleAnlasmaDosyaDto>? MevcutDosyalar { get; set; }
    }
}