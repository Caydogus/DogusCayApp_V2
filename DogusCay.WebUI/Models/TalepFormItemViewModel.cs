using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace DogusCay.WebUI.Models
{
    public class TalepFormItemViewModel
    {
        public int? AnaKategoriId { get; set; }
        public int? KategoriId { get; set; }
        public int? AltKategoriId { get; set; }
        public int ProductId { get; set; }

        public List<SelectListItem> AnaKategoriler { get; set; } = new();
        public List<SelectListItem> Kategoriler { get; set; } = new();
        public List<SelectListItem> AltKategoriler { get; set; } = new();
        public List<SelectListItem> Urunler { get; set; } = new();

        public bool ShowAltKategori { get; set; } = false;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string ErpCode { get; set; }
        public int KoliIciAdet { get; set; }       // 1 kolideki adet
        public int KoliIciToplamAdet { get; set; } // 1 kolideki adet
        public decimal KoliFiyati { get; set; }    // Hesaplanmış: Price * KoliIciAdet
        public decimal ApproximateWeightKg { get; set; } // Yaklaşık koli ağırlığı
        public decimal KoliToplamAgirligiKg { get; set; }
        public decimal ListeFiyat { get; set; }
        public decimal SonAdetFiyati { get; set; }
       
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int? PreviousKategoryId { get; set; }// ürün seçildikten sonra,ürün kategorisi değiştirirsek alt katedori ve fiyat sıfırlanmalı
        public int? PreviousAnaKategoriId { get; set; }
        public int? PreviousAltKategoriId { get; set; }

        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }
        public bool UseIskonto4 { get; set; } 

        public decimal AdetFarkDonusuTL { get; set; }
        public bool UseAdetFarkDonusuTL { get; set; }

        public decimal SabitBedel { get; set; }
        public bool UseSabitBedel { get; set; }  // Aktiflik checkbox'ı için

 
    }
}
