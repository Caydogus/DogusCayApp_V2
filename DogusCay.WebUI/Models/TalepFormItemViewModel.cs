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

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
