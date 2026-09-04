using System.Linq;
using System.Collections.Generic;
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

        // ✅ NAYA METHOD (Stock Check ko fast karne ke liye)
        public async Task<Dictionary<int, decimal>> GetAllAvailableStockAsync()
        {
            // Saare Items ka Stock ek hi baar mein nikaalo
            var items = await _context.Items.ToListAsync();

            // Har Item ke liye Total Demand calculate karo (Ek query mein)
            var totalDemands = await _context.DemandAllocations
                .Where(d => d.Status != "Completed")
                .GroupBy(d => d.ItemId)
                .Select(g => new { ItemId = g.Key, TotalDemand = g.Sum(x => x.DemandQuantity) })
                .ToListAsync();

            var demandDict = totalDemands.ToDictionary(x => x.ItemId, x => x.TotalDemand);

            // Final Available Stock Dictionary
            return items.ToDictionary(i => i.Id, i => i.Stock - (demandDict.ContainsKey(i.Id) ? demandDict[i.Id] : 0));
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