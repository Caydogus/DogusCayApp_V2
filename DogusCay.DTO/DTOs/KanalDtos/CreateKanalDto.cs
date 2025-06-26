using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.KanalDtos
{
    public class CreateKanalDto
    {
        

        [Required]
        [MaxLength(100)]
        public string KanalName { get; set; } // "DIST", "NA", "LC"

      
    }
}
