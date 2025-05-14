namespace DogusCay.WebUI.Models
{
    // TalepFormViewModel'in içindeki her bir ürün
    public class TalepFormItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
