using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.PivotDtos
{
    public class PivotRequest
    {
        public string TableName { get; set; }

        public string FilterColumn { get; set; } = "AppUserId";
        public string? FilterValue { get; set; }

        public Dictionary<string, string>? AdditionalFilters { get; set; }
    }

}
