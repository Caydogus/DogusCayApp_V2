using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Distributor
    {
        public int DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string DistributorErcKod { get; set; }
        public int KanalId { get; set; }
        public Kanal Kanal { get; set; }
        public ICollection<Point> Points { get; set; }
        public int AppUserId { get; set; } 
        public AppUser AppUser { get; set; }  
        
    }
}
