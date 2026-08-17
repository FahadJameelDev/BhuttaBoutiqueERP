using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public class FabricDemandService : IFabricDemandService
    {
        private readonly AppDbContext _context;

        public FabricDemandService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DemandAllocation>> GetAllAsync()
        {
            return await _context.DemandAllocations
                .Include(d => d.Order)
                .Include(d => d.Item)
                .ToListAsync();
        }

        public async Task<DemandAllocation?> GetByIdAsync(int id)
        {
            return await _context.DemandAllocations
                .Include(d => d.Order)
                .Include(d => d.Item)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DemandAllocation> CreateAsync(DemandAllocationViewModel model)
        {
            var demand = new DemandAllocation
            {
                OrderId = model.OrderId,
                ItemId = model.ItemId,
                DemandQuantity = model.DemandQuantity,
                AllocatedQuantity = model.AllocatedQuantity,
                Status = model.Status,
                Date = model.Date
            };

            _context.DemandAllocations.Add(demand);
            await _context.SaveChangesAsync();
            return demand;
        }

        public async Task<DemandAllocation> UpdateAsync(DemandAllocationViewModel model)
        {
            var demand = await _context.DemandAllocations.FindAsync(model.Id);
            if (demand == null)
                throw new KeyNotFoundException($"Demand with ID {model.Id} not found.");

            demand.OrderId = model.OrderId;
            demand.ItemId = model.ItemId;
            demand.DemandQuantity = model.DemandQuantity;
            demand.AllocatedQuantity = model.AllocatedQuantity;
            demand.Status = model.Status;
            demand.Date = model.Date;

            await _context.SaveChangesAsync();
            return demand;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var demand = await _context.DemandAllocations.FindAsync(id);
            if (demand == null)
                return false;

            _context.DemandAllocations.Remove(demand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DemandAllocations.AnyAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DemandAllocation>> GetByOrderIdAsync(int orderId)
        {
            return await _context.DemandAllocations
                .Include(d => d.Item)
                .Where(d => d.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DemandAllocation>> GetByItemIdAsync(int itemId)
        {
            return await _context.DemandAllocations
                .Include(d => d.Order)
                .Where(d => d.ItemId == itemId)
                .ToListAsync();
        }
    }
}