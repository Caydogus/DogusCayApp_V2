using DogusCay.Entity.Entities;
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
        public DbSet<TalepFormItem> TalepFormItems { get; set; }
        public DbSet<Distributor> Distributors { get; set; }

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

            //// TalepFormItem ilişkileri
            //modelBuilder.Entity<TalepFormItem>()
            //    .HasOne(tfi => tfi.TalepForm)
            //    .WithMany(tf => tf.TalepFormItems)
            //    .HasForeignKey(tfi => tfi.TalepFormId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TalepFormItem>()
                .HasOne(tfi => tfi.Product)
                .WithMany()
                .HasForeignKey(tfi => tfi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepFormItem>()
                .HasOne(tfi => tfi.Category)
                .WithMany()
                .HasForeignKey(tfi => tfi.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TalepFormItem>()
                .HasOne(tfi => tfi.SubCategory)
                .WithMany()
                .HasForeignKey(tfi => tfi.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal hassasiyet ayarları (TalepFormItem için)
            modelBuilder.Entity<TalepFormItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");
          //  modelBuilder.Entity<TalepFormItem>().Property(x => x.KoliFiyati).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<TalepFormItem>().Property(x => x.ApproximateWeightKg).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<TalepFormItem>().Property(x => x.Iskonto1).HasColumnType("decimal(5,2)");
            //modelBuilder.Entity<TalepFormItem>().Property(x => x.Iskonto2).HasColumnType("decimal(5,2)");
            //modelBuilder.Entity<TalepFormItem>().Property(x => x.Iskonto3).HasColumnType("decimal(5,2)");
            //modelBuilder.Entity<TalepFormItem>().Property(x => x.Iskonto4).HasColumnType("decimal(5,2)");

        }
    }
}
