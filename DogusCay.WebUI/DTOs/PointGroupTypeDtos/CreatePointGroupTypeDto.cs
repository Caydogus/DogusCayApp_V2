using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;
namespace DogusCay.WebUI.DTOs.PointGrupDtos
{
    public class CreatePointGroupTypeDto
    {
        [Required(ErrorMessage = "Nokta grup adı zorunludur.")]
        public string PointGroupTypeName { get; set; } // "YEREL ZİNCİR", "TOPTAN"

    }
}
