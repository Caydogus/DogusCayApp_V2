using System;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class CreateTalepFormItemDto
    {
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public int ProductId { get; set; }
        public string ErpCode { get; set; }
        public int Quantity { get; set; } //koli adet
        public decimal Price { get; set; }//koli fiyatı
        public int KoliIciAdet { get; set; }
        public decimal ApproximateWeightKg { get; set; }//koli ağirlığı

        public decimal? Iskonto1 { get; set; }
        public decimal? Iskonto2 { get; set; }
        public decimal? Iskonto3 { get; set; }
        public decimal? Iskonto4 { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }

}
