using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.WebUI.DTOs.TalepFormDtos
{
    public class CreateTalepFormDto
    {
        [Required]
        public int AppUserId { get; set; }

        [Required]
        public TalepTip TalepTip { get; set; }

        [Required]
        public int KanalId { get; set; }

        public int? DistributorId { get; set; }

        public int? PointGroupTypeId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir Nokta seçiniz.")]
        public int PointId { get; set; }

        public string? Note { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        [Required]
        public List<CreateTalepFormItemDto> Items { get; set; } = new();
    }
}
