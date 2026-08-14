using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public class StockInService : IStockInService
    {
        private readonly AppDbContext _context;

        public StockInService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockIn>> GetAllAsync()
        {
            return await _context.StockIns
                .Include(s => s.Item)
                .Include(s => s.Supplier)
                .ToListAsync();
        }

        public async Task<StockIn?> GetByIdAsync(int id)
        {
            return await _context.StockIns
                .Include(s => s.Item)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StockIn> CreateAsync(StockInViewModel model)
        {
            var stockIn = new StockIn
            {
                ItemId = model.ItemId,
                SupplierId = model.SupplierId,
                Quantity = model.Quantity,
                Price = model.Price,
                Date = model.Date,
                Status = model.Status
            };

            _context.StockIns.Add(stockIn);

            var item = await _context.Items.FindAsync(model.ItemId);
            if (item != null)
            {
                item.Stock += model.Quantity;
            }

            await _context.SaveChangesAsync();
            return stockIn;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stockIn = await _context.StockIns.FindAsync(id);
            if (stockIn == null)
                return false;

            var item = await _context.Items.FindAsync(stockIn.ItemId);
            if (item != null)
            {
                item.Stock -= stockIn.Quantity;
            }

            _context.StockIns.Remove(stockIn);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StockIn>> GetByItemIdAsync(int itemId)
        {
            return await _context.StockIns
                .Include(s => s.Supplier)
                .Where(s => s.ItemId == itemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockIn>> GetBySupplierIdAsync(int supplierId)
        {
            return await _context.StockIns
                .Include(s => s.Item)
                .Where(s => s.SupplierId == supplierId)
                .ToListAsync();
        }
    }
}