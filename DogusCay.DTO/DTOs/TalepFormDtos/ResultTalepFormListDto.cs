using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.TalepFormDtos
{
    public class ResultTalepFormListDto
    {
        public int TalepFormId { get; set; }
        public string KullaniciAdi { get; set; }
        public string KanalAdi { get; set; }
        public string NoktaAdi { get; set; }
        public string TalepTip { get; set; }
        public string TalepDurumu { get; set; }
        public DateTime TalepBaslangicTarihi { get; set; }
        public DateTime TalepBitisTarihi { get; set; }
    }
}
