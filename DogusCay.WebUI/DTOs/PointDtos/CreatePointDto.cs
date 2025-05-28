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
        [Required(ErrorMessage = "Nokta seçimi zorunludur.")]
        
        public string PointName { get; set; } // Firma adı, nokta adı
        public string PointErc { get; set; }
        public int KanalId { get; set; }             // ❗ zorunlu
        public int? PointGroupTypeId { get; set; }
        public int? DistributorId { get; set; }
        public int AppUserId { get; set; } // ✅ dışarıdan gelecek

    }
}
