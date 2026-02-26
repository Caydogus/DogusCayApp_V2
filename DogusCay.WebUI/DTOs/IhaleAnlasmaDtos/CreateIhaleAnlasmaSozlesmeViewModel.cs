using System.ComponentModel.DataAnnotations;

namespace DogusCay.WebUI.DTOs.IhaleAnlasmaDtos
{
    public class CreateIhaleAnlasmaSozlesmeViewModel
    {
        [Required]
        public string NoktaKod { get; set; }

        [Required(ErrorMessage = "İskonto oranı zorunludur.")]
        [Range(0, 50, ErrorMessage = "İskonto oranı 0 ile 50 arasında olmalıdır.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Ondalıklı sayı girilemez.")]
        public decimal? IskontoOrani { get; set; }
        public string? Note { get; set; }
        public List<IFormFile>? Dosyalar { get; set; }  // Resim veya PDF
    }
}
