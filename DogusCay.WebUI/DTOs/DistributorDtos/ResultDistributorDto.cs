using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.DistributorDtos
{
    public class ResultDistributorDto
    {
        public int DistributorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DistributorName { get; set; }
        public string DistributorErcKod { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
