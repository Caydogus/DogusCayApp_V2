using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class CreateTalepFormDto
    {
        public int AppUserId { get; set; }
        public int TalepTipId { get; set; }  // TalepTip bir enum ise enum direkt kullanılabilir
        public int KanalId { get; set; }
        public int PointGroupId { get; set; }
        public int PointId { get; set; }
        public DateTime TalepTarihi { get; set; }
        public int TalepDurumu { get; set; } = 0; // Enum ise int olarak gönder, backend enum'a dönüştür
        public int? OnaylayanAdminId { get; set; }

        public List<CreateTalepFormItemDto> Items { get; set; } = new();
        public string? Note { get; set; }
    }

}
