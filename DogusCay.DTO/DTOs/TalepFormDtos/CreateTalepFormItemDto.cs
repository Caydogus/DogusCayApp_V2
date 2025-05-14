using System;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class CreateTalepFormItemDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal KoliFiyati { get; set; }
        public int KoliIciAdet { get; set; }
        public decimal KoliAgirligiKg { get; set; }

        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
