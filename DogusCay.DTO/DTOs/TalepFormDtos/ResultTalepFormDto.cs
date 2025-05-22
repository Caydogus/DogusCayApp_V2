using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class ResultTalepFormDto
    {
        public int TalepFormId { get; set; }

        public string TalepTipName { get; set; }
        public string KanalName { get; set; }
        public string? DistributorName { get; set; }
        public string? PointGroupTypeName { get; set; }
        public string PointName { get; set; }

        public string UserFullName { get; set; }

        public DateTime TalepBaslangicTarihi { get; set; }
        public DateTime TalepBitisTarihi { get; set; }

        public string TalepDurumuText { get; set; }
        public string? Note { get; set; }

        public List<ResultTalepFormItemDto> Items { get; set; } = new();
    }

}
