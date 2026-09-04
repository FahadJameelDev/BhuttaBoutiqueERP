using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> CreateAsync(CustomerViewModel model)
        {
            var customer = new Customer
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                Type = model.Type,
                Status = model.Status
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(CustomerViewModel model)
        {
            var customer = await _context.Customers.FindAsync(model.Id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {model.Id} not found.");

            customer.Name = model.Name;
            customer.Phone = model.Phone;
            customer.Email = model.Email;
            customer.Address = model.Address;
            customer.Type = model.Type;
            customer.Status = model.Status;

            await _context.SaveChangesAsync();
            return customer;
        }

        // ✅ FINAL FIXED DELETE (Production Module Included)
        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            // 1. Is customer ke saare Orders ke IDs nikalo
            var orderIds = await _context.Orders
                .Where(o => o.CustomerId == id)
                .Select(o => o.Id)
                .ToListAsync();

            // 2. Production Module (OrderLifecycles, FabricIssues, ProductionLogs) delete karo
            _context.OrderLifecycles.RemoveRange(_context.OrderLifecycles.Where(x => orderIds.Contains(x.OrderId)));
            _context.FabricIssues.RemoveRange(_context.FabricIssues.Where(x => orderIds.Contains(x.OrderId)));
            _context.ProductionLogs.RemoveRange(_context.ProductionLogs.Where(x => orderIds.Contains(x.OrderId)));

            // 3. DemandAllocations, OrderItems delete karo
            _context.DemandAllocations.RemoveRange(_context.DemandAllocations.Where(x => orderIds.Contains(x.OrderId)));
            _context.OrderItems.RemoveRange(_context.OrderItems.Where(x => orderIds.Contains(x.OrderId)));

            // 4. Child records delete karo
            _context.Measurements.RemoveRange(_context.Measurements.Where(x => x.CustomerId == id || orderIds.Contains(x.OrderId)));
            _context.Payments.RemoveRange(_context.Payments.Where(x => orderIds.Contains(x.OrderId)));
            _context.BulkOrders.RemoveRange(_context.BulkOrders.Where(x => x.CustomerId == id));
            _context.Orders.RemoveRange(_context.Orders.Where(x => x.CustomerId == id));

            // 5. Ab Customer delete karo
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == phone);
        }
    }
}