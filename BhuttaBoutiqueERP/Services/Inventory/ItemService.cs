using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> CreateAsync(ItemViewModel model)
        {
            var item = new Item
            {
                Code = model.Code,
                Name = model.Name,
                Category = model.Category,
                Stock = model.Stock,
                Price = model.Price,
                Unit = model.Unit,
                ReorderLevel = model.ReorderLevel,
                Status = model.Status
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateAsync(ItemViewModel model)
        {
            var item = await _context.Items.FindAsync(model.Id);
            if (item == null)
                throw new KeyNotFoundException($"Item with ID {model.Id} not found.");

            item.Code = model.Code;
            item.Name = model.Name;
            item.Category = model.Category;
            item.Price = model.Price;
            item.Unit = model.Unit;
            item.ReorderLevel = model.ReorderLevel;
            item.Status = model.Status;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return false;

            // 1. StockIns se is item ke records remove karein (agar hain)
            var stockIns = await _context.StockIns.Where(s => s.ItemId == id).ToListAsync();
            if (stockIns.Any())
            {
                _context.StockIns.RemoveRange(stockIns);
            }

            // 2. StockOuts se is item ke records remove karein (agar hain)
            var stockOuts = await _context.StockOuts.Where(s => s.ItemId == id).ToListAsync();
            if (stockOuts.Any())
            {
                _context.StockOuts.RemoveRange(stockOuts);
            }

            // 3. StockReturns se is item ke records remove karein (agar hain)
            var stockReturns = await _context.StockReturns.Where(s => s.ItemId == id).ToListAsync();
            if (stockReturns.Any())
            {
                _context.StockReturns.RemoveRange(stockReturns);
            }

            // 4. Ab Item ko safely delete karein
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Items.AnyAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> GetLowStockItemsAsync()
        {
            return await _context.Items
                .Where(i => i.Stock <= i.ReorderLevel)
                .ToListAsync();
        }
    }
}