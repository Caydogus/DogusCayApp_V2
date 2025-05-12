using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;
namespace DogusCay.WebUI.DTOs.PointGrupDtos
{
    public class CreatePointGroupDto
    {
        [Required(ErrorMessage = "Nokta grup adı zorunludur.")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Lütfen bir kanal seçiniz.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kanal seçiniz.")]
        public int KanalId { get; set; }

    }
}
