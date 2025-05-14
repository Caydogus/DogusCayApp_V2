using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DogusCay.WebUI.Models
{
    public class TalepFormViewModel
    {
        public TalepTip TalepTip { get; set; }

        // Seçilen değerler (form input)
        public int KanalId { get; set; }
        public int PointGroupId { get; set; }
        public int PointId { get; set; }

        // Dropdownlar için liste verileri
        public List<SelectListItem> Kanallar { get; set; } = new();
        public List<SelectListItem> PointGruplar { get; set; } = new();
        public List<SelectListItem> Noktalar { get; set; } = new();

        public List<TalepFormItemViewModel> Items { get; set; } = new();
    }
}
