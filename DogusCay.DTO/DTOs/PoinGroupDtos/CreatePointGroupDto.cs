using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.PointGrupDtos
{
    public class CreatePointGroupDto
    {
        [Required,MaxLength(100)]
        public string GroupName { get; set; } // YEREL ZİNCİR, TOPTAN, ULUSAL ZİNCİR...

        public int KanalId { get; set; }           // Foreign key

       
    }
}
