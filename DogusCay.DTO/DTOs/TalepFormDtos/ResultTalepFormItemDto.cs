using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class ResultTalepFormItemDto
    {
        public int TalepFormItemId { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public decimal KoliFiyati { get; set; }
        public decimal KoliAgirligiKg { get; set; }

        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }

}
