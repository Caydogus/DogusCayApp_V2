using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DogusCay.Entity.Entities.IhaleAnlasma
{
    public class IhaleAnlasma
    {
        public int AppUserId { get; set; }
        public string BolgeMuduru { get; set; }
        public string DistKod { get; set; }
        public string DistAdi { get; set; }
        public string NoktaKod { get; set; }  // Unique identifier
        public string NoktaAdi { get; set; }
    }
}