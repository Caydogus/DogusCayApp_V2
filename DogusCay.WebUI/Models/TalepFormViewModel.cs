using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogusCay.WebUI.Models
{
    public class TalepFormViewModel
    {
        // 🔹 Talep tipi (Insert / MalYukleme)
        [Required]
        public TalepTip TalepTip { get; set; }

        // 🔹 Seçilen değerler
        public int KanalId { get; set; }

        // Eğer DIST ise:
        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; }

        // Her durumda
        public int PointId { get; set; }

        // 🔹 Dropdown verileri
        public List<SelectListItem> Kanallar { get; set; } = new();
        public List<SelectListItem> Distributors { get; set; } = new();
        public List<SelectListItem> PointGruplar { get; set; } = new();
        public List<SelectListItem> Noktalar { get; set; } = new();

        // 🔹 Ürün giriş alanları
        public List<TalepFormItemViewModel> Items { get; set; } = new();

        public string? Note { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
