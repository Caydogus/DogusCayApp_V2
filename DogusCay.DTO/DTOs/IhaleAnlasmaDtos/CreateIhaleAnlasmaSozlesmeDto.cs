using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.IhaleAnlasmaDtos
{
    public class CreateIhaleAnlasmaSozlesmeDto
    {
        public string NoktaKod { get; set; }        // Hangi nokta için
        public decimal IskontoOrani { get; set; }
        public string? Note { get; set; }
    }
}
