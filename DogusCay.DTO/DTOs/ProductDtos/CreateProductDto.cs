using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.ProductDtos
{
    public class CreateProductDto
    {

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ErpCode { get; set; }

        // Kategori bilgisi (alt kategori olabilir)
        public int CategoryId { get; set; }

        // Birim tipi: Koli mi, Kilo mu?
        public int UnitTypeId { get; set; }

        // Her bir ürün için yaklaşık kilo karşılığı (koli ise tonaj hesabı için)
        public decimal? ApproximateWeightKg { get; set; }

        // Aktif/pasif ürün durumu
        public bool IsShown { get; set; } = true;

        // Satışlar ile ilişki
      

    }
}
