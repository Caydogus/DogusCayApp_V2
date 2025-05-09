using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.RegionDtos
{
    public class CreateRegionDto
    {

        [Required]
        [MaxLength(100)]
        public string RegionName { get; set; }

        // Bu bölgenin müdürü (tek kullanıcı)
        public int? ManagerUserId { get; set; }



    }
}
