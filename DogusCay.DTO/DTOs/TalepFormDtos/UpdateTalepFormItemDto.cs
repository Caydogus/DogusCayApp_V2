using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    // Güncelleme sırasında kullanılacak ürün detayları
    public class UpdateTalepFormItemDto
    {
        public int TalepFormItemId { get; set; }

        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public int ProductId { get; set; }

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
