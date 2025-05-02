using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class SalesPoint
    {
        public int SalesPointId { get; set; }

        [Required]
        [MaxLength(150)]
        public string SalesPointName { get; set; } // Örn: Migros İzmir Şubesi

        // Hangi bölgede
        public int RegionId { get; set; }
        public Region Region { get; set; }

        // Hangi kanala ait (örn: Ulusal zincirler, Yerel marketler)
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

        // Aktiflik durumu
        public bool IsActive { get; set; } = true;

        // Navigasyon: Yapılan satışlar
        public ICollection<Sale> Sales { get; set; }
    }

}
