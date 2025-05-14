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
        public int PointGroupId { get; set; }
        public int PointId { get; set; }

        public DateTime TalepTarihi { get; set; }
        public int TalepDurumu { get; set; } = 0;
        public int? OnaylayanAdminId { get; set; }

        public List<UpdateTalepFormItemDto> Items { get; set; } = new(); // ✅ Doğru DTO tipi
        public string? Note { get; set; }
    }
}
