using DogusCay.Entity.Entities;
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

            // ✅ Distributor → Kanal ilişkisi
            modelBuilder.Entity<Distributor>()
                .HasOne(d => d.Kanal)
                .WithMany(k => k.Distributors) // ← BU ÖNEMLİ
                .HasForeignKey(d => d.KanalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal tipleri için hassasiyet ayarları
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Sale>()
                .Property(s => s.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Sale>()
                .Property(s => s.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalNetPrice)
                .HasColumnType("decimal(18,2)");
            // TalepForm → Distributor
            modelBuilder.Entity<TalepForm>()
                .HasOne(tf => tf.Distributor)
                .WithMany()
                .HasForeignKey(tf => tf.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepForm>()
                .Property(p => p.BrutTotal)
                .HasPrecision(18, 2);

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

        }


    }
}

