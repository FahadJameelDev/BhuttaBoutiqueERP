using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Reports.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class DashboardReportService : IDashboardReportService
    {
        private readonly AppDbContext _context;

        public DashboardReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardOverviewViewModel> GetDashboardOverviewAsync()
        {
            var today = DateTime.Today;

            decimal totalRevenue = await _context.Payments
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            int totalOrders = await _context.Orders.CountAsync();
            int pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending" || o.Status == "Processing");
            int lowStockCount = await _context.Items.CountAsync(i => i.Stock <= i.ReorderLevel);
            int activeEmployees = await _context.Employees.CountAsync(e => e.Status == "Active");
            int presentEmployeesToday = await _context.Attendances.CountAsync(a => a.Date.Date == today && a.Status == "Present");

            return new DashboardOverviewViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                LowStockItemsCount = lowStockCount,
                ActiveEmployees = activeEmployees,
                PresentEmployeesToday = presentEmployeesToday
            };
        }
    }
}