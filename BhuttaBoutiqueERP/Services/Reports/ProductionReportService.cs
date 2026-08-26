using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class ProductionReportService : IProductionReportService
    {
        private readonly AppDbContext _context;

        public ProductionReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductionReportVM> GetProductionSummaryReportAsync()
        {
            // Database se production logs fetch karna (safe property access ke sath)
            var productionList = await _context.ProductionLogs
                .Include(p => p.Order)
                .Select(p => new ProductionItemRowVM
                {
                    Id = p.Id,
                    OrderNumber = p.Order != null ? p.Order.OrderNumber : "N/A",
                    DesignName = "Custom Garment",
                    AssignedTailor = "Assigned Staff",
                    CurrentStage = "Stitching Phase", // Safe fallback text
                    Status = "In Progress 🔄"
                })
                .ToListAsync();

            int totalOrders = productionList.Count;

            var reportVm = new ProductionReportVM
            {
                TotalOrdersInProduction = totalOrders,
                CompletedOrdersCount = 0, // Aapki DB logic ke mutabiq adjust ho sakta hai
                TotalFabricIssuedMeters = 0,
                Items = productionList
            };

            return reportVm;
        }
    }
}