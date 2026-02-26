namespace DogusCay.DTO.DTOs.IhaleAnlasmaDtos
{
    public class ResultIhaleAnlasmaDosyaDto
    {
        public int IhaleAnlasmaDosyaId { get; set; }
        public string DosyaAdi { get; set; }
        public string DosyaYolu { get; set; }          // Görüntüleme/indirme linki
        public string DosyaTipi { get; set; }          // image/jpeg veya application/pdf
        public long DosyaBoyutu { get; set; }
        public int SayfaSirasi { get; set; }
    }
}
