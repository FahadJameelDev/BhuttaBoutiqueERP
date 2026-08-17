using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;

namespace BhuttaBoutiqueERP.Services.Production
{
    public class StockCheckService : IStockCheckService
    {
        private readonly AppDbContext _context;

        public StockCheckService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckAvailabilityAsync(int itemId, decimal quantity)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return false;

            var totalDemand = await _context.DemandAllocations
                .Where(d => d.ItemId == itemId && d.Status != "Completed")
                .SumAsync(d => d.DemandQuantity);

            return (item.Stock - totalDemand) >= quantity;
        }

        public async Task<decimal> GetAvailableStockAsync(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return 0;

            var totalDemand = await _context.DemandAllocations
                .Where(d => d.ItemId == itemId && d.Status != "Completed")
                .SumAsync(d => d.DemandQuantity);

            return item.Stock - totalDemand;
        }

        public async Task<bool> ValidateDemandAsync(int orderId)
        {
            var demands = await _context.DemandAllocations
                .Where(d => d.OrderId == orderId)
                .ToListAsync();

            if (!demands.Any())
                return false;

            foreach (var demand in demands)
            {
                var available = await GetAvailableStockAsync(demand.ItemId);
                if (available < demand.DemandQuantity)
                    return false;
            }

            return true;
        }
    }
}