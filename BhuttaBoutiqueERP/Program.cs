using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Services.Auth;
using BhuttaBoutiqueERP.Services.HR;
using BhuttaBoutiqueERP.Services.Inventory;
using BhuttaBoutiqueERP.Services.Production;
using BhuttaBoutiqueERP.Services.Reports;
using BhuttaBoutiqueERP.Services.Services;
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
// 📌 3️⃣ INVENTORY SERVICES REGISTRATION
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
// 📌 PRODUCTION SERVICES
// ==========================================
builder.Services.AddScoped<IFabricDemandService, FabricDemandService>();
builder.Services.AddScoped<IDemandListService, DemandListService>();
builder.Services.AddScoped<IStockCheckService, StockCheckService>();
builder.Services.AddScoped<IFabricAllocationService, FabricAllocationService>();
builder.Services.AddScoped<IFabricIssueService, FabricIssueService>();
builder.Services.AddScoped<IOrderLifecycleService, OrderLifecycleService>();

// Fully qualified namespace for Production Report to prevent ambiguity
builder.Services.AddScoped<BhuttaBoutiqueERP.Services.Production.IProductionReportService, BhuttaBoutiqueERP.Services.Production.ProductionReportService>();

// ==========================================
// 📌 HR & PAYROLL SERVICES
// ==========================================
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IAdvanceService, AdvanceService>();
builder.Services.AddScoped<ICommissionRateService, CommissionRateService>();

// ==========================================
// 📌 REPORTS MODULE SERVICES REGISTRATION
// ==========================================
builder.Services.AddScoped<IDashboardReportService, DashboardReportService>();
builder.Services.AddScoped<ISalesReportService, SalesReportService>();
builder.Services.AddScoped<IInventoryReportService, InventoryReportService>();
builder.Services.AddScoped<BhuttaBoutiqueERP.Services.Reports.IProductionReportService, BhuttaBoutiqueERP.Services.Reports.ProductionReportService>();
builder.Services.AddScoped<IHRReportService, HRReportService>();
builder.Services.AddScoped<IFinancialReportService, FinancialReportService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

// Controllers and Global Exception & Audit Filters Registration
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<BhuttaBoutiqueERP.Filters.GlobalExceptionFilter>();
    options.Filters.Add<BhuttaBoutiqueERP.Filters.GlobalAuditFilter>(); // Yeh line yahan add kar do
});

var app = builder.Build();

// 🚀 4️⃣ SMART DATABASE SEEDER (5 Locked Roles & Default Users with Specific Emails)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // 1. Array of the 5 Locked Roles
        string[] lockedRoles = {
            "Admin",
            "InventoryManager",
            "ProductionSupervisor",
            "BookingClerk",
            "HRManager"
        };

        foreach (var roleName in lockedRoles)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                context.Roles.Add(new Role { Name = roleName });
            }
        }
        context.SaveChanges();

        // 2. Default Password Hash (Password for all default users: Admin@123)
        string defaultPassword = "Admin@123";
        string hashedPassword;
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(defaultPassword));
            hashedPassword = Convert.ToBase64String(hashedBytes);
        }

        // 3. Define Specific Users with their respective Roles and Emails
        var defaultUsers = new[]
        {
            new { Name = "Super Admin", Email = "admin@bhuttaboutique.com", Role = "Admin" },
            new { Name = "Inventory Manager", Email = "inventory@bhuttaboutique.com", Role = "InventoryManager" },
            new { Name = "Production Supervisor", Email = "production@bhuttaboutique.com", Role = "ProductionSupervisor" },
            new { Name = "Booking Clerk", Email = "booking@bhuttaboutique.com", Role = "BookingClerk" },
            new { Name = "HR Manager", Email = "hr@bhuttaboutique.com", Role = "HRManager" }
        };

        // 4. Insert users if they don't already exist
        foreach (var userInfo in defaultUsers)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Email == userInfo.Email);
            if (existingUser == null)
            {
                var roleObj = context.Roles.First(r => r.Name == userInfo.Role);

                var newUser = new User
                {
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    PasswordHash = hashedPassword,
                    IsActive = true,
                    RoleId = roleObj.Id
                };

                context.Users.Add(newUser);
            }
        }
        context.SaveChanges();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles and default users.");
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