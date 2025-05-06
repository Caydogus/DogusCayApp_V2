using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Region
    {
       
        public int RegionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RegionName { get; set; }
   
        // Navigasyon: Bölgeye bağlı yöneticiler
        public ICollection<AppUser> Users { get; set; } 

        // Navigasyon: Bölgedeki satış noktaları
        public ICollection<PointGroup> PointGroups { get; set; } 
    }

}

