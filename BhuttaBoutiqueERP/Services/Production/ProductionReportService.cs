using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public class ProductionReportService : IProductionReportService
    {
        private readonly AppDbContext _context;

        public ProductionReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductionReportViewModel> GetReportDataAsync()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var totalOrdersInProduction = await _context.OrderLifecycles
                .Select(o => o.OrderId)
                .Distinct()
                .CountAsync();

            var pendingOrders = await _context.OrderLifecycles
                .Where(o => o.Status == "Pending" || o.Status == "InProgress")
                .Select(o => o.OrderId)
                .Distinct()
                .CountAsync();

            var completedOrders = await _context.OrderLifecycles
                .Where(o => o.Status == "Completed")
                .Select(o => o.OrderId)
                .Distinct()
                .CountAsync();

            var cuttingCount = await _context.OrderLifecycles
                .Where(o => o.Stage == "Cutting" && o.Status != "Completed")
                .CountAsync();

            var stitchingCount = await _context.OrderLifecycles
                .Where(o => o.Stage == "Stitching" && o.Status != "Completed")
                .CountAsync();

            var embroideryCount = await _context.OrderLifecycles
                .Where(o => o.Stage == "Embroidery" && o.Status != "Completed")
                .CountAsync();

            var finishingCount = await _context.OrderLifecycles
                .Where(o => o.Stage == "Finishing" && o.Status != "Completed")
                .CountAsync();

            var qcCount = await _context.OrderLifecycles
                .Where(o => o.Stage == "QC" && o.Status != "Completed")
                .CountAsync();

            var recentActivities = await _context.ProductionLogs
                .OrderByDescending(p => p.Date)
                .Take(10)
                .Select(p => new ProductionActivity
                {
                    OrderId = p.OrderId,
                    OrderNumber = p.Order != null ? p.Order.OrderNumber : "N/A",
                    Stage = p.Stage,
                    Status = p.Status,
                    Date = p.Date,
                    Notes = p.Notes
                })
                .ToListAsync();

            return new ProductionReportViewModel
            {
                TotalOrdersInProduction = totalOrdersInProduction,
                PendingOrders = pendingOrders,
                CompletedOrders = completedOrders,
                CuttingCount = cuttingCount,
                StitchingCount = stitchingCount,
                EmbroideryCount = embroideryCount,
                FinishingCount = finishingCount,
                QCCount = qcCount,
                RecentActivities = recentActivities
            };
        }

        public async Task<ProductionReportViewModel> GetReportByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var baseReport = await GetReportDataAsync();

            var activities = await _context.ProductionLogs
                .Where(p => p.Date >= startDate && p.Date <= endDate)
                .OrderByDescending(p => p.Date)
                .Take(10)
                .Select(p => new ProductionActivity
                {
                    OrderId = p.OrderId,
                    OrderNumber = p.Order != null ? p.Order.OrderNumber : "N/A",
                    Stage = p.Stage,
                    Status = p.Status,
                    Date = p.Date,
                    Notes = p.Notes
                })
                .ToListAsync();

            baseReport.RecentActivities = activities;
            baseReport.ReportPeriod = $"{startDate:dd MMM yyyy} - {endDate:dd MMM yyyy}";

            return baseReport;
        }
    }
}