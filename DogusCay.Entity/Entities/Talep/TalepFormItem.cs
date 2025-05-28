using System;

namespace DogusCay.Entity.Entities.Talep
{
    public class TalepFormItem
    {
        public int TalepFormItemId { get; set; }

        public int TalepFormId { get; set; }
        public TalepForm TalepForm { get; set; }

        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public Category? Category { get; set; }
        public Category? SubCategory { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ErpCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public decimal ApproximateWeightKg { get; set; }

        public decimal? Iskonto1 { get; set; }
        public decimal? Iskonto2 { get; set; }
        public decimal? Iskonto3 { get; set; }
        public decimal? Iskonto4 { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
