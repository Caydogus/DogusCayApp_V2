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
        [Required(ErrorMessage = "Nokta seçimi zorunludur.")]
        public int PointId { get; set; }
        public string PointErc { get; set; }
        public string PointName { get; set; }
        
        public int? DistributorId { get; set; }
      
        public Distributor? Distributor { get; set; }

        public int? PointGroupTypeId { get; set; } // Foreign Key
        public PointGroupType? PointGroupType { get; set; }

        public int? KanalId { get; set; }
        public Kanal? Kanal { get; set; }
        public int AppUserId { get; set; } // Bölge Müdürü
        public AppUser AppUser { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}
