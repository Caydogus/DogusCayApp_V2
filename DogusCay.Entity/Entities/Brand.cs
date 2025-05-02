using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; }

        // Marka aktif/pasif durumda olabilir
        public bool IsActive { get; set; } = true;

        // Navigasyon: Bu markaya ait ürünler
        public ICollection<Product> Products { get; set; } 
    }

}
