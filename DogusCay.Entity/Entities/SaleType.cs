using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class SaleType
    {
        public int SaleTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SaleTypeName { get; set; } // Örn: Normal, Kampanya, İade, Numune

        public ICollection<Sale> Sales { get; set; }
    }

}
