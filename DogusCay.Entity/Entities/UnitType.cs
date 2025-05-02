using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class UnitType
    {
        public int UnitTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnitTypeName { get; set; } // Örn: "Kilo", "Koli"

        // Navigasyon: Bu birime sahip ürünler
        public ICollection<Product> Products { get; set; }
    }

}
