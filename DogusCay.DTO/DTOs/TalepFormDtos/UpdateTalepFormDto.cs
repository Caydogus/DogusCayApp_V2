using System;
using System.Collections.Generic;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class UpdateTalepFormDto
    {
        public int TalepFormId { get; set; }

        public int AppUserId { get; set; }
        public int TalepTipId { get; set; }
        public int KanalId { get; set; }

        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; }
        public int PointId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? Note { get; set; }

        public int TalepDurumu { get; set; } = 0;
        public int? OnaylayanAdminId { get; set; }

        public List<UpdateTalepFormItemDto> Items { get; set; } = new();
    }
}
