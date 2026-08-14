using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public class WastageService : IWastageService
    {
        private readonly AppDbContext _context;

        public WastageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wastage>> GetAllAsync()
        {
            return await _context.Wastages
                .Include(w => w.Item)
                .ToListAsync();
        }

        public async Task<Wastage?> GetByIdAsync(int id)
        {
            return await _context.Wastages
                .Include(w => w.Item)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Wastage> CreateAsync(WastageViewModel model)
        {
            var wastage = new Wastage
            {
                ItemId = model.ItemId,
                Quantity = model.Quantity,
                Reason = model.Reason,
                Date = model.Date,
                Status = model.Status
            };

            _context.Wastages.Add(wastage);

            var item = await _context.Items.FindAsync(model.ItemId);
            if (item != null)
            {
                item.Stock -= model.Quantity;
            }

            await _context.SaveChangesAsync();
            return wastage;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var wastage = await _context.Wastages.FindAsync(id);
            if (wastage == null)
                return false;

            var item = await _context.Items.FindAsync(wastage.ItemId);
            if (item != null)
            {
                item.Stock += wastage.Quantity;
            }

            _context.Wastages.Remove(wastage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Wastage>> GetByItemIdAsync(int itemId)
        {
            return await _context.Wastages
                .Where(w => w.ItemId == itemId)
                .ToListAsync();
        }
    }
}