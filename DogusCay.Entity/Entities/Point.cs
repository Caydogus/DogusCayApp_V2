using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Point
    {
        public int PointId { get; set; }

        [Required, MaxLength(150)]
        public string PointName { get; set; } // Firma adı, nokta adı

        [Required]
        public decimal Total { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? PointGroupId { get; set; }
        public PointGroup? PointGroup { get; set; }
        public int KanalId { get; set; }        // ❗ zorunlu
        public Kanal Kanal { get; set; }
        public int AppUserId { get; set; } // Bölge Müdürü
        public AppUser AppUser { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}
