using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.ProductDtos
{
    public class UpdateProductDto
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
        public decimal Price { get; set; }



    }
}
