

using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Entity.Entities.IhaleAnlasma
{
    public class IhaleAnlasmaSozlesme
    {
        public int IhaleAnlasmaSozlesmeId { get; set; }
        public string NoktaKod { get; set; }            // IHALE_ANLASMA_TABLOSU ile bağlantı
        public int AppUserId { get; set; }
        public decimal IskontoOrani { get; set; }
        public DateTime CreateDate { get; set; }
        public TalepDurumu TalepDurumu { get; set; } = TalepDurumu.Bekliyor;
        public int? OnaylayanAdminId { get; set; }
        public string? Note { get; set; }

        // Navigation
        public AppUser AppUser { get; set; }
        public AppUser? OnaylayanAdmin { get; set; }
        public ICollection<IhaleAnlasmaDosya> Dosyalar { get; set; }
    }
}