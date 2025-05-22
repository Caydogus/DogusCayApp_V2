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
        public string KanalName { get; set; } // "DIST", "NA", "LC"

        public ICollection<Distributor> Distributors { get; set; }
        public ICollection<Point> Points { get; set; } // NA & LC için doğrudan ilişki
    }

}