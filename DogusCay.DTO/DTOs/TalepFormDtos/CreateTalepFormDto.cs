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
        public int TalepTipId { get; set; }
        public int KanalId { get; set; }
        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; }
        public int PointId { get; set; }

        public DateTime TalepBaslangicTarihi { get; set; }
        public DateTime TalepBitisTarihi { get; set; }

        public string? Note { get; set; }

        public List<CreateTalepFormItemDto> Items { get; set; } = new();
    }

}
