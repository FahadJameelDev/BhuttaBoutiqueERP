using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class ServiceCatalogService : IServiceCatalogService
    {
        private readonly AppDbContext _context;

        public ServiceCatalogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceCatalog>> GetAllAsync()
        {
            return await _context.ServiceCatalogs.ToListAsync();
        }

        public async Task<ServiceCatalog?> GetByIdAsync(int id)
        {
            return await _context.ServiceCatalogs.FindAsync(id);
        }

        public async Task<ServiceCatalog> CreateAsync(ServiceCatalogViewModel model)
        {
            var service = new ServiceCatalog
            {
                Name = model.Name,
                Category = model.Category,
                Price = model.Price,
                Time = model.Time,
                Complexity = model.Complexity,
                FabricRequire = model.FabricRequire,
                Status = model.Status
            };

            _context.ServiceCatalogs.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<ServiceCatalog> UpdateAsync(ServiceCatalogViewModel model)
        {
            var service = await _context.ServiceCatalogs.FindAsync(model.Id);
            if (service == null)
                throw new KeyNotFoundException($"Service with ID {model.Id} not found.");

            service.Name = model.Name;
            service.Category = model.Category;
            service.Price = model.Price;
            service.Time = model.Time;
            service.Complexity = model.Complexity;
            service.FabricRequire = model.FabricRequire;
            service.Status = model.Status;

            await _context.SaveChangesAsync();
            return service;
        }

        // ✅ FINAL FIXED DELETE (Production Module Included)
        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.ServiceCatalogs.FindAsync(id);
            if (service == null)
                return false;

            // 1. Is service ke saare Orders ke IDs nikalo
            var orderIds = await _context.Orders
                .Where(o => o.ServiceId == id)
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
            _context.Measurements.RemoveRange(_context.Measurements.Where(x => orderIds.Contains(x.OrderId)));
            _context.Payments.RemoveRange(_context.Payments.Where(x => orderIds.Contains(x.OrderId)));
            _context.CommissionRates.RemoveRange(_context.CommissionRates.Where(x => x.ServiceId == id));
            _context.Orders.RemoveRange(_context.Orders.Where(x => x.ServiceId == id));

            // 5. Ab ServiceCatalog delete karo
            _context.ServiceCatalogs.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ServiceCatalogs.AnyAsync(s => s.Id == id);
        }
    }
}