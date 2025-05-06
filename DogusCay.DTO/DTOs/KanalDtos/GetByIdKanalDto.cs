using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.ChannelDtos
{
    public class GetByIdKanalDto
    {
        public int KanalId { get; set; }

        [Required]
        [MaxLength(100)]
        public string KanalName { get; set; } // Örn: Ulusal Zincir, Online

        // Navigasyon: Bu kanala bağlı satış noktaları
        //public ICollection<SalesPoint> SalesPoints { get; set; }
    }
}
