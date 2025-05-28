using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.PointDtos
{
    public class UpdatePointDto
    {
        [Required(ErrorMessage = "Nokta seçimi zorunludur.")]

        public int PointId { get; set; }

        [Required, MaxLength(150)]
        public string PointName { get; set; } // Firma adı, nokta adı
        public string PointErc { get; set; }
        public int KanalId { get; set; }
        public int? PointGroupTypeId { get; set; }
       

    }
}
