using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities.Talep
{
    public class TalepForm
    {
        public int TalepFormId { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public TalepTip TalepTip { get; set; }

        public int KanalId { get; set; }
        public Kanal Kanal { get; set; }

        public int PointGroupId { get; set; }
        public PointGroup PointGroup { get; set; }

        public int PointId { get; set; }
        public Point Point { get; set; }

        public DateTime TalepTarihi { get; set; }
        public TalepDurumu TalepDurumu { get; set; } = TalepDurumu.Bekliyor;

        public int? OnaylayanAdminId { get; set; }
        public AppUser? OnaylayanAdmin { get; set; }

        public ICollection<TalepFormItem> Items { get; set; }
        public string? Note { get; set; }
    }

}