using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.PointGrupDtos
{
    public class ResultPointGroupDto
    {
        public int PointGroupId { get; set; }
        public string GroupName { get; set; }

        public int KanalId { get; set; }
        public Kanal Kanal { get; set; } // ✅ Burada Kanal nesnesi var
    }
}
