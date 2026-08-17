using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Services.Auth;

using BhuttaBoutiqueERP.Services.Inventory; // 📌 Yeh namespace lazmi add karein taake services recognize hon
using BhuttaBoutiqueERP.Services.Production;  // ✅ PRODUCTION NAMESPACE
using BhuttaBoutiqueERP.Services.Services; // 📌 Services Module Namespace
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Database Connection Register karo
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2️⃣ Session Services Add karo
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// ── AUTH SERVICES ──
builder.Services.AddScoped<IUserService, UserService>();

// ==========================================
// 📌 3️⃣ INVENTORY SERVICES REGISTRATION (Yahan add ki hain)
// ==========================================
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IStockInService, StockInService>();
builder.Services.AddScoped<IStockOutService, StockOutService>();
builder.Services.AddScoped<IStockReturnService, StockReturnService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IWastageService, WastageService>();

// ==========================================
// 📌 3.1️⃣ SERVICES MODULE SERVICES REGISTRATION
// ==========================================
builder.Services.AddScoped<IServiceCatalogService, ServiceCatalogService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBulkOrderService, BulkOrderService>();
builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IDesignLibraryService, DesignLibraryService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IServiceReportService, ServiceReportService>();
// ==========================================
// 📌 PRODUCTION SERVICES ✅ NEW
// ==========================================
builder.Services.AddScoped<IFabricDemandService, FabricDemandService>();
builder.Services.AddScoped<IDemandListService, DemandListService>();
builder.Services.AddScoped<IStockCheckService, StockCheckService>();
builder.Services.AddScoped<IFabricAllocationService, FabricAllocationService>();
builder.Services.AddScoped<IFabricIssueService, FabricIssueService>();
builder.Services.AddScoped<IOrderLifecycleService, OrderLifecycleService>();
builder.Services.AddScoped<IProductionReportService, ProductionReportService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🚀 4️⃣ SMART DATABASE SEEDER (Creates Role & Admin Safely)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // Step A: Ensure 'Admin' role exists first
        var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role { Name = "Admin" };
            context.Roles.Add(adminRole);
            context.SaveChanges(); // Save to get the generated Role Id
        }

        // Step B: Check if Admin user exists
        string adminEmail = "admin@bhuttaboutique.com";
        var existingUser = context.Users.FirstOrDefault(u => u.Email == adminEmail);

        if (existingUser == null)
        {
            string password = "Admin@123";
            string hashedPassword;
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                hashedPassword = Convert.ToBase64String(hashedBytes);
            }

            var adminUser = new User
            {
                Name = "Super Admin",
                Email = adminEmail,
                PasswordHash = hashedPassword,
                IsActive = true,
                RoleId = adminRole.Id // Dynamically assigned correct Role ID
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();
app.Run();