using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.PaymentTypeDtos
{
    public class CreatePaymentTypeDto
    {
        [Required]
        [MaxLength(50)]
        public string PaymentTypeName { get; set; } // Örn: Nakit, Vadeli, Çek

    }
}
