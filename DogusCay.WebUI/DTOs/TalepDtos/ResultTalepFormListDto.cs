namespace DogusCay.WebUI.DTOs.TalepDtos
{
    public class ResultTalepFormListDto
    {
        public int TalepFormId { get; set; }
        public string KullaniciAdi { get; set; }
        public string KanalAdi { get; set; }
        public string NoktaAdi { get; set; }
        public string TalepTip { get; set; }
        public string TalepDurumu { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
