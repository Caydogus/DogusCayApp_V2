using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;
namespace DogusCay.WebUI.DTOs.PointDtos
{
    public class CreatePointDto
    {
       

        [Required, MaxLength(150)]
        public string PointName { get; set; } // Firma adı, nokta adı
        public int KanalId { get; set; }             // ❗ zorunlu

        public int? PointGroupId { get; set; }
        //public int AppUserId { get; set; } // Bölge Müdürü

    }
}
