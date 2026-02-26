using DogusCay.Entity.Entities;
using DogusCay.Entity.Entities.IhaleAnlasma;
using DogusCay.Entity.Entities.MalYuklemeTalep;
using DogusCay.Entity.Entities.Talep;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Context
{
    public class DogusCayContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DogusCayContext(DbContextOptions<DogusCayContext> options) : base(options) { }

        // DbSet tanımları
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PointGroupType> PointGroupTypes { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Kanal> Kanals { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<TalepForm> TalepForms { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<MalYuklemeTalepForm> MalYuklemeTalepForms { get; set; }
        public DbSet<MalYuklemeTalepFormDetail> MalYuklemeTalepFormDetails { get; set; }
        // İhale Anlaşma ile ilgili DbSet'ler 10.02.2026 tarihinde eklendi
        // DbSet'ler — sadece yeni tablolar için DbSet, IhaleAnlasma için de lazım ama migration'dan hariç
        public DbSet<IhaleAnlasma> IhaleAnlasmalar { get; set; }
        public DbSet<IhaleAnlasmaSozlesme> IhaleAnlasmaSozlesmeler { get; set; }
        public DbSet<IhaleAnlasmaDosya> IhaleAnlasmaDosyalar { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Point ilişkileri
            modelBuilder.Entity<Point>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Points)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Point>()
                .HasOne(p => p.PointGroupType)
                .WithMany(pg => pg.Points)
                .HasForeignKey(p => p.PointGroupTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Sale ilişkileri
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Point)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.PointId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sales)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.PaymentType)
                .WithMany(pt => pt.Sales)
                .HasForeignKey(s => s.PaymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // TalepForm ilişkileri
            modelBuilder.Entity<TalepForm>()
                .HasOne(tf => tf.Point)
                .WithMany()
                .HasForeignKey(tf => tf.PointId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepForm>()
                .HasOne(tf => tf.Kanal)
                .WithMany()
                .HasForeignKey(tf => tf.KanalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepForm>()
                .HasOne(tf => tf.PointGroupType)
                .WithMany()
                .HasForeignKey(tf => tf.PointGroupTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepForm>()
                .HasOne(tf => tf.AppUser)
                .WithMany()
                .HasForeignKey(tf => tf.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepForm>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // ❗ veya DeleteBehavior.NoAction

            modelBuilder.Entity<TalepForm>()
                .HasOne(tf => tf.OnaylayanAdmin)
                .WithMany()
                .HasForeignKey(tf => tf.OnaylayanAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Distributor  Kanal ilişkisi
            modelBuilder.Entity<Distributor>()
                .HasOne(d => d.Kanal)
                .WithMany(k => k.Distributors) // ← BU ÖNEMLİ
                .HasForeignKey(d => d.KanalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal tipleri için hassasiyet ayarları - MAL YÜKLEME TALEP FORMU GENEL TOPLAMLARI
            modelBuilder.Entity<MalYuklemeTalepForm>()
                .Property(mf => mf.BrutTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .Property(mf => mf.Maliyet)
                .HasPrecision(18, 2); // Yüzde olduğu için hassasiyeti ona göre ayarlayın

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .Property(mf => mf.ToplamAgirlikKg)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .Property(mf => mf.Total) // Net Total
                .HasPrecision(18, 2);

            // Decimal tipleri için hassasiyet ayarları - MAL YÜKLEME TALEP FORMU DETAYLARI
            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.ApproximateWeightKg)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.BrutTutar)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.Discount1)
                .HasPrecision(18, 2); // Yüzde olduğu için hassasiyet önemli

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.Discount2)
                .HasPrecision(18, 2); // Yüzde olduğu için hassasiyet önemli

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.FixedPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.Maliyet)
                .HasPrecision(18, 2); // Yüzde olduğu için hassasiyet önemli

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.NetAdetFiyat)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.NetTutar)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .Property(d => d.Price)
                .HasPrecision(18, 2); // Ürün Fiyatı

            // Decimal tipleri için hassasiyet ayarları - Product
            modelBuilder.Entity<Product>()
                .Property(p => p.ApproximateWeightKg)
                .HasPrecision(18, 2);

            // Decimal tipleri için hassasiyet ayarları - Sale
            modelBuilder.Entity<Sale>()
                .Property(s => s.DiscountPercentage)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.IskontoAltiPercentage)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.NetPrice)
                .HasPrecision(18, 2);

            // Decimal tipleri için hassasiyet ayarları - TalepForm
            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.AdetFarkDonusuTL)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.ApproximateWeightKg)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Iskonto1)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Iskonto2)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Iskonto3)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Iskonto4)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.KoliToplamAgirligiKg)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.ListeFiyat)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Maliyet)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.SabitBedelTL)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.SonAdetFiyati)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TalepForm>()
                .Property(tf => tf.Total)
                .HasPrecision(18, 2);

            // TalepForm Distributor zaten vardı, düzenlenmedi

            // MalYuklemeTalepForm FK ilişkileri (Cascade çakışmasını önlemek için Restrict kullandık)
            modelBuilder.Entity<MalYuklemeTalepForm>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .HasOne(x => x.Kanal)
                .WithMany()
                .HasForeignKey(x => x.KanalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .HasOne(x => x.Distributor)
                .WithMany()
                .HasForeignKey(x => x.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .HasOne(x => x.PointGroupType)
                .WithMany()
                .HasForeignKey(x => x.PointGroupTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MalYuklemeTalepForm>()
                .HasOne(x => x.Point)
                .WithMany()
                .HasForeignKey(x => x.PointId)
                .OnDelete(DeleteBehavior.Restrict);

            // Detail ilişkileri
            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .HasOne(d => d.MalYuklemeTalepForm)
                .WithMany(f => f.MalYuklemeTalepFormDetails)
                .HasForeignKey(d => d.MalYuklemeTalepFormId)
                .OnDelete(DeleteBehavior.Cascade); // Ana form silinirse detayları da sil

            
            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .HasOne(d => d.Product)
                .WithMany() // Eğer Product tarafında MalYuklemeTalepFormDetail için bir ICollection yoksa WithMany() kullanırız
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // <<-- ÖNEMLİ DEĞİŞİKLİK BURADA: Cascade yerine NO ACTION

          
            // Category ile olan ilişki (CategoryId)
            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); // Category silinirse detay silinmesin

            // SubCategory ile olan ilişki (SubCategoryId)
            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .HasOne(d => d.SubCategory)
                .WithMany()
                .HasForeignKey(d => d.SubCategoryId)
                .OnDelete(DeleteBehavior.NoAction); // SubCategory silinirse detay silinmesin

            // SubSubCategory ile olan ilişki (SubSubCategoryId)
            modelBuilder.Entity<MalYuklemeTalepFormDetail>()
                .HasOne(d => d.SubSubCategory)
                .WithMany()
                .HasForeignKey(d => d.SubSubCategoryId)
                .OnDelete(DeleteBehavior.NoAction); // SubSubCategory silinirse detay silinmesin

            // =============================================
            // İHALE ANLAŞMA — Linked server tablosu
            // Migration'dan hariç tut, sadece EF sorgulaması için
            // =============================================
            modelBuilder.Entity<IhaleAnlasma>(entity =>
            {
                entity.ToTable("IHALE_ANLASMA_TABLOSU", t => t.ExcludeFromMigrations());
                entity.HasKey(e => e.NoktaKod);
                entity.Property(e => e.BolgeMuduru).HasColumnName("BOLGE_MUDURU");
                entity.Property(e => e.DistKod).HasColumnName("DIST_KOD");
                entity.Property(e => e.DistAdi).HasColumnName("DIST_ADI");
                entity.Property(e => e.NoktaKod).HasColumnName("NOKTA_KOD");
                entity.Property(e => e.NoktaAdi).HasColumnName("NOKTA_ADI");
            });

            // =============================================
            // İHALE ANLAŞMA SÖZLEŞME — Yeni tablo
            // =============================================
            modelBuilder.Entity<IhaleAnlasmaSozlesme>(entity =>
            {
                entity.HasIndex(e => e.NoktaKod).IsUnique();

                entity.Property(e => e.IskontoOrani).HasPrecision(18, 2);

                entity.HasOne(e => e.AppUser)
                    .WithMany()
                    .HasForeignKey(e => e.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.OnaylayanAdmin)
                    .WithMany()
                    .HasForeignKey(e => e.OnaylayanAdminId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =============================================
            // İHALE ANLAŞMA DOSYA — Yeni tablo
            // =============================================
            modelBuilder.Entity<IhaleAnlasmaDosya>(entity =>
            {
                entity.HasOne(d => d.IhaleAnlasmaSozlesme)
                    .WithMany(s => s.Dosyalar)
                    .HasForeignKey(d => d.IhaleAnlasmaSozlesmeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}


