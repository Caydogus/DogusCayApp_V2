using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Channel
    {
        public int ChannelId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ChannelName { get; set; } // Örn: Ulusal Zincir, Online

        // Navigasyon: Bu kanala bağlı satış noktaları
        public ICollection<SalesPoint> SalesPoints { get; set; }
    }

}