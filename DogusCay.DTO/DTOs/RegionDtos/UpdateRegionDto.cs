using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.RegionDtos
{
    public class UpdateRegionDto
    {
        public int RegionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RegionName { get; set; }

        // Bu bölgenin müdürü (tek kullanıcı)
        public int? ManagerUserId { get; set; }
    }
}
