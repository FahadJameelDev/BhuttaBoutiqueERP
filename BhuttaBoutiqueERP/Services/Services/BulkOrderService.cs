using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class BulkOrderService : IBulkOrderService
    {
        private readonly AppDbContext _context;

        public BulkOrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BulkOrder>> GetAllAsync()
        {
            return await _context.BulkOrders
                .Include(b => b.Customer)
                .ToListAsync();
        }

        public async Task<BulkOrder?> GetByIdAsync(int id)
        {
            return await _context.BulkOrders
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BulkOrder> CreateAsync(BulkOrderViewModel model)
        {
            var bulkOrder = new BulkOrder
            {
                CustomerId = model.CustomerId,
                TotalItems = model.TotalItems,
                TotalPrice = model.TotalPrice,
                Discount = model.Discount,
                OrderDate = model.OrderDate,
                DeliveryDate = model.DeliveryDate,
                Status = model.Status
            };

            _context.BulkOrders.Add(bulkOrder);
            await _context.SaveChangesAsync();
            return bulkOrder;
        }

        public async Task<BulkOrder> UpdateAsync(BulkOrderViewModel model)
        {
            var bulkOrder = await _context.BulkOrders.FindAsync(model.Id);
            if (bulkOrder == null)
                throw new KeyNotFoundException($"Bulk order with ID {model.Id} not found.");

            bulkOrder.TotalItems = model.TotalItems;
            bulkOrder.TotalPrice = model.TotalPrice;
            bulkOrder.Discount = model.Discount;
            bulkOrder.DeliveryDate = model.DeliveryDate;
            bulkOrder.Status = model.Status;

            await _context.SaveChangesAsync();
            return bulkOrder;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bulkOrder = await _context.BulkOrders.FindAsync(id);
            if (bulkOrder == null)
                return false;

            _context.BulkOrders.Remove(bulkOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.BulkOrders.AnyAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BulkOrder>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.BulkOrders
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();
        }
    }
}