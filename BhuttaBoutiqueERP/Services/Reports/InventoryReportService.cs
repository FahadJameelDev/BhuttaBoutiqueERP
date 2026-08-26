using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class InventoryReportService : IInventoryReportService
    {
        private readonly AppDbContext _context;

        public InventoryReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryReportVM> GetInventorySummaryReportAsync()
        {
            var itemsList = await _context.Items
                .Select(i => new InventoryItemRowVM
                {
                    Id = i.Id,
                    Code = i.Code ?? string.Empty,
                    Name = i.Name ?? string.Empty,
                    Category = i.Category ?? string.Empty,
                    Stock = i.Stock,
                    ReorderLevel = i.ReorderLevel,
                    Status = i.Stock <= 0 ? "Critical 🔴" : (i.Stock <= i.ReorderLevel ? "Low ⚠️" : "Normal")
                })
                .ToListAsync();

            decimal totalWastage = 0;
            if (_context.Wastages != null)
            {
                totalWastage = await _context.Wastages
                    .SumAsync(w => (decimal?)(w.Quantity * 0)) ?? 0;
            }

            var reportVm = new InventoryReportVM
            {
                TotalItems = itemsList.Count,
                LowStockCount = itemsList.Count(x => x.Status.Contains("Low") || x.Status.Contains("Critical")),
                TotalWastageValue = totalWastage,
                Items = itemsList
            };

            return reportVm;
        }
    }
}