using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Models.Entities;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }

        // Inventory Module DbSets (7 Tables)
        public DbSet<Item> Items { get; set; }
        public DbSet<StockIn> StockIns { get; set; }
        public DbSet<StockOut> StockOuts { get; set; }
        public DbSet<StockReturn> StockReturns { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Wastage> Wastages { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Relationships & Restrict Cascade Delete (Auth)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================================
            // INVENTORY MODULE RELATIONSHIPS & RESTRICTIONS
            // ==========================================

            // Item -> OrderItems (Cascade Delete as defined in locked structure)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Item -> StockIn (Restrict)
            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.Item)
                .WithMany(i => i.StockIns)
                .HasForeignKey(si => si.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Supplier -> StockIn (Restrict)
            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.Supplier)
                .WithMany(s => s.StockIns)
                .HasForeignKey(si => si.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item -> StockOut (Restrict)
            modelBuilder.Entity<StockOut>()
                .HasOne(so => so.Item)
                .WithMany(i => i.StockOuts)
                .HasForeignKey(so => so.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // StockOut -> StockReturn (Restrict)
            modelBuilder.Entity<StockReturn>()
                .HasOne(sr => sr.StockOut)
                .WithMany(so => so.StockReturns)
                .HasForeignKey(sr => sr.StockOutId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item -> StockReturn (Restrict)
            modelBuilder.Entity<StockReturn>()
                .HasOne(sr => sr.Item)
                .WithMany(i => i.StockReturns)
                .HasForeignKey(sr => sr.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item -> Wastage (Restrict)
            modelBuilder.Entity<Wastage>()
                .HasOne(w => w.Item)
                .WithMany(i => i.Wastages)
                .HasForeignKey(w => w.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}