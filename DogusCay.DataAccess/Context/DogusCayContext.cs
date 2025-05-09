using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Context
{
    public class DogusCayContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DogusCayContext(DbContextOptions<DogusCayContext> options) : base(options) { }

        // Tablolar
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PointGroup> PointGroups { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Kanal> Kanals { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppUser - Point: 1 to many
            modelBuilder.Entity<Point>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Points)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kanal - PointGroup: 1 to many
            modelBuilder.Entity<PointGroup>()
                .HasOne(pg => pg.Kanal)
                .WithMany(k => k.PointGroups)
                .HasForeignKey(pg => pg.KanalId)
                .OnDelete(DeleteBehavior.Restrict);

            // PointGroup - Point: 1 to many
            modelBuilder.Entity<Point>()
                .HasOne(p => p.PointGroup)
                .WithMany(pg => pg.Points)
                .HasForeignKey(p => p.PointGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Point)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.PointId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sales)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.PaymentType)
                .WithMany(pt => pt.Sales)
                .HasForeignKey(s => s.PaymentTypeId);

            modelBuilder.Entity<Region>()
                .HasOne(r => r.ManagerUser)
                .WithMany()
                .HasForeignKey(r => r.ManagerUserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Region>()
                .HasOne(r => r.ManagerUser)
                .WithMany()
                .HasForeignKey(r => r.ManagerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}