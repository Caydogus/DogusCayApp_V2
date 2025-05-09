using System.ComponentModel.DataAnnotations;

namespace DogusCay.WebUI.DTOs.ProductDtos
{
    public class ResultProductDto
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ErpCode { get; set; }

        // Kategori bilgisi
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }  // DTO'da sadeleştirilmiş şekilde
        public string ParentCategoryName { get; set; }
        // Birim tipi
        public int UnitTypeId { get; set; }
        public string UnitTypeName { get; set; }  // DTO'da sadeleştirilmiş şekilde

        public decimal? ApproximateWeightKg { get; set; }

        public bool IsShown { get; set; } = true;
        public int Price { get; set; }
    }
}
