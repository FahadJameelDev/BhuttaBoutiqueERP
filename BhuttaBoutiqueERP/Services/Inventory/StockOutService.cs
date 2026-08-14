using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public class StockOutService : IStockOutService
    {
        private readonly AppDbContext _context;

        public StockOutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockOut>> GetAllAsync()
        {
            return await _context.StockOuts
                .Include(s => s.Item)
                .ToListAsync();
        }

        public async Task<StockOut?> GetByIdAsync(int id)
        {
            return await _context.StockOuts
                .Include(s => s.Item)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StockOut> CreateAsync(StockOutViewModel model)
        {
            var stockOut = new StockOut
            {
                ItemId = model.ItemId,
                OrderId = model.OrderId,
                Quantity = model.Quantity,
                IssuedTo = model.IssuedTo,
                Date = model.Date,
                Status = model.Status
            };

            _context.StockOuts.Add(stockOut);

            var item = await _context.Items.FindAsync(model.ItemId);
            if (item != null)
            {
                item.Stock -= model.Quantity;
            }

            await _context.SaveChangesAsync();
            return stockOut;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stockOut = await _context.StockOuts.FindAsync(id);
            if (stockOut == null)
                return false;

            var item = await _context.Items.FindAsync(stockOut.ItemId);
            if (item != null)
            {
                item.Stock += stockOut.Quantity;
            }

            _context.StockOuts.Remove(stockOut);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StockOut>> GetByItemIdAsync(int itemId)
        {
            return await _context.StockOuts
                .Where(s => s.ItemId == itemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockOut>> GetByOrderIdAsync(int orderId)
        {
            return await _context.StockOuts
                .Where(s => s.OrderId == orderId)
                .ToListAsync();
        }
    }
}