using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.PointGrupDtos
{
    public class UpdatePointGroupTypeDto
    {
        public int PointGroupTypeId { get; set; }
        public string PointGroupTypeName { get; set; } // "YEREL ZİNCİR", "TOPTAN"
        public ICollection<Point> Points { get; set; }


    }
}
