using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public class StockReturnService : IStockReturnService
    {
        private readonly AppDbContext _context;

        public StockReturnService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockReturn>> GetAllAsync()
        {
            return await _context.StockReturns
                .Include(s => s.Item)
                .Include(s => s.StockOut)
                .ToListAsync();
        }

        public async Task<StockReturn?> GetByIdAsync(int id)
        {
            return await _context.StockReturns
                .Include(s => s.Item)
                .Include(s => s.StockOut)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StockReturn> CreateAsync(StockReturnViewModel model)
        {
            var stockReturn = new StockReturn
            {
                ItemId = model.ItemId,
                StockOutId = model.StockOutId,
                Quantity = model.Quantity,
                Date = model.Date,
                Status = model.Status
            };

            _context.StockReturns.Add(stockReturn);

            var item = await _context.Items.FindAsync(model.ItemId);
            if (item != null)
            {
                item.Stock += model.Quantity;
            }

            await _context.SaveChangesAsync();
            return stockReturn;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stockReturn = await _context.StockReturns.FindAsync(id);
            if (stockReturn == null)
                return false;

            var item = await _context.Items.FindAsync(stockReturn.ItemId);
            if (item != null)
            {
                item.Stock -= stockReturn.Quantity;
            }

            _context.StockReturns.Remove(stockReturn);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StockReturn>> GetByItemIdAsync(int itemId)
        {
            return await _context.StockReturns
                .Where(s => s.ItemId == itemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockReturn>> GetByStockOutIdAsync(int stockOutId)
        {
            return await _context.StockReturns
                .Where(s => s.StockOutId == stockOutId)
                .ToListAsync();
        }
    }
}