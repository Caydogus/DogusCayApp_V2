namespace DogusCay.WebUI.DTOs.TalepDtos
{
    public class ResultTalepFormItemDto
    {
        public string ProductName { get; set; }
        public string ErpCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public decimal ApproximateWeightKg { get; set; }

        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }
        public decimal Total { get; set; }
        public decimal KoliToplamAgirligiKg { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
