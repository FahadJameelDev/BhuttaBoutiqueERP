using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Service)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Service)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateAsync(OrderViewModel model)
        {
            var remaining = model.Total - model.Advance;

            var order = new Order
            {
                OrderNumber = model.OrderNumber,
                CustomerId = model.CustomerId,
                ServiceId = model.ServiceId,
                Total = model.Total,
                Advance = model.Advance,
                Remaining = remaining,
                PaymentMethod = model.PaymentMethod,
                Status = model.Status,
                OrderDate = model.OrderDate,
                DeliveryDate = model.DeliveryDate
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(OrderViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {model.Id} not found.");

            order.CustomerId = model.CustomerId;
            order.ServiceId = model.ServiceId;
            order.Total = model.Total;
            order.Advance = model.Advance;
            order.Remaining = model.Total - model.Advance;
            order.PaymentMethod = model.PaymentMethod;
            order.Status = model.Status;
            order.DeliveryDate = model.DeliveryDate;

            await _context.SaveChangesAsync();
            return order;
        }

        // ✅ FIXED DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            // 1. Saare child records pehle delete karo
            _context.Payments.RemoveRange(_context.Payments.Where(x => x.OrderId == id));
            _context.Measurements.RemoveRange(_context.Measurements.Where(x => x.OrderId == id));
            _context.DemandAllocations.RemoveRange(_context.DemandAllocations.Where(x => x.OrderId == id));
            _context.FabricIssues.RemoveRange(_context.FabricIssues.Where(x => x.OrderId == id));
            _context.OrderLifecycles.RemoveRange(_context.OrderLifecycles.Where(x => x.OrderId == id));
            _context.ProductionLogs.RemoveRange(_context.ProductionLogs.Where(x => x.OrderId == id));

            // 2. Ab Order delete karo
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.Service)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Service)
                .Where(o => o.Status == "Pending" || o.Status == "Processing")
                .ToListAsync();
        }
    }
}