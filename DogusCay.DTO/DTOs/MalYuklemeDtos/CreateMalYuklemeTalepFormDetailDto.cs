using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.MalYuklemeDtos
{
    public class CreateMalYuklemeTalepFormDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount1 { get; set; }      // İskonto 1 (%)
        public decimal? Discount2 { get; set; }      // İskonto 2 (%)
        public decimal? FixedPrice { get; set; }     // Sabit Bedel
        public decimal? NetTutar { get; set; }       // Hesaplanan Net Tutar
        public decimal? NetAdetFiyat { get; set; }   // Hesaplanan Net Adet Fiyatı
        public decimal? BrutTutar { get; set; }      // Hesaplanan Brüt Tutar
        public decimal? Maliyet { get; set; }        // Hesaplanan Maliyet (%)


    }

}
