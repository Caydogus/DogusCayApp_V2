using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Kanal
    {
        public int KanalId { get; set; }

        [Required]
        [MaxLength(100)]
        public string KanalName { get; set; } // Örn: Ulusal Zincir, Online

        // Navigasyon: Bu kanala bağlı satış noktaları
        public ICollection<PointGroup> PointGroups { get; set; }
        public ICollection<Point> Points { get; set; }
    }

}