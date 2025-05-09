using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.PointDtos
{
    public class ResultPointDto
    {
        public int PointId { get; set; }

        [Required, MaxLength(150)]
        public string PointName { get; set; } // Firma adı, nokta adı

        [Required]
        public decimal Total { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int PointGroupId { get; set; }
        public PointGroup PointGroup { get; set; }

        public int AppUserId { get; set; } // Bölge Müdürü
        public AppUser AppUser { get; set; }
    }
}
