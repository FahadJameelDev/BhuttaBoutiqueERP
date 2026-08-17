using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class ServiceReportService : IServiceReportService
    {
        private readonly AppDbContext _context;

        public ServiceReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceReportViewModel> GetReportDataAsync()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            // ── Widgets ──
            var totalOrders = await _context.Orders.CountAsync();
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending" || o.Status == "Processing");
            var totalRevenue = await _context.Orders.SumAsync(o => (decimal?)o.Total) ?? 0;
            var monthlyRevenue = await _context.Orders
                .Where(o => o.OrderDate >= startOfMonth)
                .SumAsync(o => (decimal?)o.Total) ?? 0;

            // ── Top 5 Services ──
            var topServices = await _context.Orders
                .GroupBy(o => o.ServiceId)
                .Select(g => new
                {
                    ServiceId = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(o => o.Total)
                })
                .OrderByDescending(g => g.Revenue)
                .Take(5)
                .Join(_context.ServiceCatalogs,
                    g => g.ServiceId,
                    s => s.Id,
                    (g, s) => new ServiceStat
                    {
                        ServiceName = s.Name,
                        OrderCount = g.Count,
                        TotalRevenue = g.Revenue
                    })
                .ToListAsync();

            // ── Recent 10 Orders ──
            var recentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .Select(o => new OrderSummary
                {
                    OrderId = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.Customer != null ? o.Customer.Name : "N/A",
                    ServiceName = o.Service != null ? o.Service.Name : "N/A",
                    Total = o.Total,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    DeliveryDate = o.DeliveryDate
                })
                .ToListAsync();

            return new ServiceReportViewModel
            {
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                TopServices = topServices,
                RecentOrders = recentOrders
            };
        }
    }
}