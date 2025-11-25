using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.PivotDtos
{
    public class PivotDataRow
    {
        // Dinamik kolonları tutacak yapı
        // Örn: "BÖLGE_MÜDÜRÜ" → "01_METE"
        //      "AY" → 1
        //      "TOPLAM_NETCIRO" → 294000
        public Dictionary<string, object> Columns { get; set; } = new();
    }
}
