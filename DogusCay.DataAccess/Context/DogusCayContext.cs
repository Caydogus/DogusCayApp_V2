using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Context
{
    public class DogusCayContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public DogusCayContext(DbContextOptions<DogusCayContext> options) : base(options) { }

        // Tablolar
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
      

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Tüm foreign key'ler için default delete behavior
        //    foreach (var relationship in modelBuilder.Model.GetEntityTypes()
        //        .SelectMany(e => e.GetForeignKeys()))
        //    {
        //        relationship.DeleteBehavior = DeleteBehavior.Restrict;
        //    }

        //    // 1. Category Konfigürasyonu (Self-referencing)
        //    modelBuilder.Entity<Category>(entity =>
        //    {
        //        entity.HasKey(c => c.CategoryId);

        //        entity.HasMany(c => c.SubCategories)
        //            .WithOne(c => c.ParentCategory)
        //            .HasForeignKey(c => c.ParentCategoryId);

        //        entity.HasMany(c => c.Products)
        //            .WithOne(p => p.Category)
        //            .HasForeignKey(p => p.CategoryId);
        //    });

        //    // 2. Product Konfigürasyonu
        //    modelBuilder.Entity<Product>(entity =>
        //    {
        //        entity.HasOne(p => p.Brand)
        //            .WithMany(b => b.Products)
        //            .HasForeignKey(p => p.BrandId)
        //            .IsRequired();

        //        entity.HasOne(p => p.UnitType)
        //            .WithMany(u => u.Products)
        //            .HasForeignKey(p => p.UnitTypeId)
        //            .IsRequired();
        //    });

        //    // 3. SalesPoint Konfigürasyonu
        //    modelBuilder.Entity<SalesPoint>(entity =>
        //    {
        //        entity.HasOne(sp => sp.Region)
        //            .WithMany(r => r.SalesPoints)
        //            .HasForeignKey(sp => sp.RegionId);

        //        entity.HasOne(sp => sp.Channel)
        //            .WithMany(c => c.SalesPoints)
        //            .HasForeignKey(sp => sp.ChannelId);
        //    });

        //    // 4. Sale Konfigürasyonu
        //    modelBuilder.Entity<Sale>(entity =>
        //    {
        //        entity.HasOne(s => s.Product)
        //            .WithMany(p => p.Sales)
        //            .HasForeignKey(s => s.ProductId);

        //        entity.HasOne(s => s.SalesPoint)
        //            .WithMany(sp => sp.Sales)
        //            .HasForeignKey(s => s.SalesPointId);

              

        //        entity.HasOne(s => s.PaymentType)
        //            .WithMany(pt => pt.Sales)
        //            .HasForeignKey(s => s.PaymentTypeId);

        //        entity.HasOne(s => s.SaleType)
        //            .WithMany(st => st.Sales)
        //            .HasForeignKey(s => s.SaleTypeId);
        //    });

        //    // 5. User Konfigürasyonu
        //    //modelBuilder.Entity<AppUser>(entity =>
        //    //{
        //    //    entity.HasKey(u => u.Id);

        //    //    entity.HasOne(u => u.Role)
        //    //        .WithMany(r => r.Users)
        //    //        .HasForeignKey(u => u.RoleId);

        //    //    // Dikkat: DbSet adı Managers ama entity User
        //    //    entity.ToTable("Users"); // Veritabanında tablo adını Users olarak sabitle
        //    //});

        //    // 6. Unique Alanlar
        //    modelBuilder.Entity<Category>()
        //        .HasIndex(c => c.CategoryName)
        //        .IsUnique();

        //    modelBuilder.Entity<Brand>()
        //        .HasIndex(b => b.BrandName)
        //        .IsUnique();

        //    modelBuilder.Entity<UnitType>()
        //        .HasIndex(u => u.UnitTypeName)
        //        .IsUnique();
        //}
    }
}