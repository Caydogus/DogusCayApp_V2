using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// DogusCay.Entity/Entities/IhaleAnlasma/IhaleAnlasmaDosya.cs

namespace DogusCay.Entity.Entities.IhaleAnlasma
{
    public class IhaleAnlasmaDosya
    {
        public int IhaleAnlasmaDosyaId { get; set; }
        public int IhaleAnlasmaSozlesmeId { get; set; }
        public string DosyaAdi { get; set; }
        public string DosyaYolu { get; set; }
        public string DosyaTipi { get; set; }
        public long DosyaBoyutu { get; set; }
        public int SayfaSirasi { get; set; }
        public DateTime YuklenmeTarihi { get; set; }

        // Navigation
        public IhaleAnlasmaSozlesme IhaleAnlasmaSozlesme { get; set; }
    }
}