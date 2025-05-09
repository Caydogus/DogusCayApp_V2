using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.PaymentTypeDtos
{
    public class ResultPaymentTypeDto
    {
        public int PaymentTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentTypeName { get; set; } // Örn: Nakit, Vadeli, Çek

        // Navigasyon: Bu ödeme tipiyle yapılan satışlar
        public ICollection<Sale> Sales { get; set; }
    }
}
