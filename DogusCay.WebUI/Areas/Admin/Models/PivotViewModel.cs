using System.Collections.Generic;

namespace DogusCay.WebUI.Areas.Admin.Models
{
    public class PivotViewModel
    {
        // Pivot başlığı
        public string Title { get; set; } = "";

        // UI’da kullanıcıya gösterilecek tablo listesi
        public List<string> Tables { get; set; } = new List<string>();

        // UI’da seçilen tablo
        public string SelectedTable { get; set; }

        // Pivot datası (dinamik kolon formatı)
        public List<Dictionary<string, object>> Rows { get; set; }
            = new List<Dictionary<string, object>>();
    }
}
