using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ── AUTH MODULE ──
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        // ── INVENTORY MODULE (7 Tables) ──
        public DbSet<Item> Items { get; set; }
        public DbSet<StockIn> StockIns { get; set; }
        public DbSet<StockOut> StockOuts { get; set; }
        public DbSet<StockReturn> StockReturns { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Wastage> Wastages { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // ── SERVICES MODULE (7 Tables) ──
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceCatalog> ServiceCatalogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<BulkOrder> BulkOrders { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<DesignLibrary> DesignLibraries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        // ── PRODUCTION MODULE (4 Tables) ──
        public DbSet<DemandAllocation> DemandAllocations { get; set; }
        public DbSet<FabricIssue> FabricIssues { get; set; }
        public DbSet<OrderLifecycle> OrderLifecycles { get; set; }
        public DbSet<ProductionLog> ProductionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── AUTH CONFIGURATION ──
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── INVENTORY CONFIGURATION ──
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.Item)
                .WithMany(i => i.StockIns)
                .HasForeignKey(si => si.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.Supplier)
                .WithMany(s => s.StockIns)
                .HasForeignKey(si => si.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOut>()
                .HasOne(so => so.Item)
                .WithMany(i => i.StockOuts)
                .HasForeignKey(so => so.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockReturn>()
                .HasOne(sr => sr.StockOut)
                .WithMany(so => so.StockReturns)
                .HasForeignKey(sr => sr.StockOutId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockReturn>()
                .HasOne(sr => sr.Item)
                .WithMany(i => i.StockReturns)
                .HasForeignKey(sr => sr.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wastage>()
                .HasOne(w => w.Item)
                .WithMany(i => i.Wastages)
                .HasForeignKey(w => w.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── SERVICES CONFIGURATION ──

            // Unique Constraints
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // 1. Customer → Orders (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Customer → BulkOrders (One-to-Many)
            modelBuilder.Entity<BulkOrder>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.BulkOrders)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Customer → Measurements (One-to-Many)
            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.Customer)
                .WithMany(c => c.Measurements)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. ServiceCatalog → Orders (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. Order → Payments (One-to-Many) - Cascade
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Order → Measurements (One-to-Many) ✅ FIXED
            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.Order)
                .WithMany(o => o.Measurements)  // ✅ WithMany (One-to-Many)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            // ── PRODUCTION CONFIGURATION (Safe & Optimized) ──

            // 1. DemandAllocation Relations
            modelBuilder.Entity<DemandAllocation>()
                .HasOne(da => da.Order)
                .WithMany()
                .HasForeignKey(da => da.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DemandAllocation>()
                .HasOne(da => da.Item)
                .WithMany()
                .HasForeignKey(da => da.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. FabricIssue Relations
            modelBuilder.Entity<FabricIssue>()
                .HasOne(fi => fi.Order)
                .WithMany()
                .HasForeignKey(fi => fi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FabricIssue>()
                .HasOne(fi => fi.Item)
                .WithMany()
                .HasForeignKey(fi => fi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FabricIssue>()
                .HasOne(fi => fi.Employee)
                .WithMany()
                .HasForeignKey(fi => fi.IssuedTo)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. OrderLifecycle Relations
            modelBuilder.Entity<OrderLifecycle>()
                .HasOne(ol => ol.Order)
                .WithMany()
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLifecycle>()
                .HasOne(ol => ol.Employee)
                .WithMany()
                .HasForeignKey(ol => ol.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. ProductionLog Relations
            modelBuilder.Entity<ProductionLog>()
                .HasOne(pl => pl.Order)
                .WithMany()
                .HasForeignKey(pl => pl.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}