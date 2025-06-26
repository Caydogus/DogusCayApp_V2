using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities.Talep;



namespace DogusCay.Entity.Entities.MalYuklemeTalep
{
    public class MalYuklemeTalepForm
    {
        public int MalYuklemeTalepFormId { get; set; }
        public int AppUserId { get; set; }
        public int KanalId { get; set; }
        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; }
        public int PointId { get; set; }
        public DateTime CreateDate { get; set; } //tarih otomatik eklenecek
        public TalepTip TalepTip { get; set; } = TalepTip.MalYukleme;
        public TalepDurumu TalepDurumu { get; set; } = TalepDurumu.Bekliyor;
        public int? OnaylayanAdminId { get; set; }
        public AppUser? OnaylayanAdmin { get; set; }
        public AppUser AppUser { get; set; }
        public Kanal Kanal { get; set; }
        public Distributor? Distributor { get; set; }
        public PointGroupType? PointGroupType { get; set; }
        public Point Point { get; set; }

        public decimal? Total { get; set; }              // Net toplam (iskontosuz bile olsa)
        public decimal? BrutTotal { get; set; }          // Brüt toplam
        public decimal? ToplamAgirlikKg { get; set; }    // Tüm ürünlerin toplam ağırlığı
        public decimal? Maliyet { get; set; }            // (Brüt - Net) / Brüt × 100

        public ICollection<MalYuklemeTalepFormDetail>? MalYuklemeTalepFormDetails { get; set; }
    }
}


