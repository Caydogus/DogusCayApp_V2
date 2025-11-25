using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.PivotDtos
{
    public class PivotResponse
    {
        // API → WebUI’ya dönen ham pivot datası
        public List<Dictionary<string, object>> Rows { get; set; }
    }
}
