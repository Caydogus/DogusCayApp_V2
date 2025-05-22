using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.WebUI.DTOs.PointDtos
{
    public class UpdatePointDto
    {
        [Required, MaxLength(150)]
        public string PointName { get; set; } // Firma adı, nokta adı
        public string PointErc { get; set; }
        public int PointId { get; set; }
        public int KanalId { get; set; }             // ❗ zorunlu
        public int? PointGroupTypeId { get; set; }
        public int? DistributorId { get; set; }
        public int AppUserId { get; set; } // ✅ dışarıdan gelecek

    }
}
