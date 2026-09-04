using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public class FabricAllocationService : IFabricAllocationService
    {
        private readonly AppDbContext _context;

        public FabricAllocationService(AppDbContext context)
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

        public async Task<DemandAllocation> AllocateAsync(FabricAllocationViewModel model)
        {
            var demand = await _context.DemandAllocations.FindAsync(model.DemandId);
            if (demand == null)
                throw new KeyNotFoundException($"Demand with ID {model.DemandId} not found.");

            var item = await _context.Items.FindAsync(demand.ItemId);
            if (item == null)
                throw new KeyNotFoundException($"Item with ID {demand.ItemId} not found.");

            if (item.Stock < model.AllocatedQuantity)
                throw new InvalidOperationException("Insufficient stock for allocation.");

            demand.AllocatedQuantity = model.AllocatedQuantity;
            demand.Status = "Allocated";
            demand.Date = DateTime.Now;  // ✅ YEH LINE ADD KI HAI

            item.Stock -= model.AllocatedQuantity;

            await _context.SaveChangesAsync();
            return demand;
        }

        public async Task<DemandAllocation> UpdateAllocationAsync(DemandAllocationViewModel model)
        {
            var demand = await _context.DemandAllocations.FindAsync(model.Id);
            if (demand == null)
                throw new KeyNotFoundException($"Demand with ID {model.Id} not found.");

            var item = await _context.Items.FindAsync(demand.ItemId);
            if (item != null)
            {
                item.Stock += demand.AllocatedQuantity;
            }

            demand.AllocatedQuantity = model.AllocatedQuantity;
            demand.Status = model.Status;

            if (item != null)
            {
                item.Stock -= model.AllocatedQuantity;
            }

            await _context.SaveChangesAsync();
            return demand;
        }

        public async Task<bool> DeallocateAsync(int id)
        {
            var demand = await _context.DemandAllocations.FindAsync(id);
            if (demand == null)
                return false;

            var item = await _context.Items.FindAsync(demand.ItemId);
            if (item != null)
            {
                item.Stock += demand.AllocatedQuantity;
            }

            demand.AllocatedQuantity = 0;
            demand.Status = "Pending";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DemandAllocation>> GetByOrderIdAsync(int orderId)
        {
            return await _context.DemandAllocations
                .Include(d => d.Item)
                .Where(d => d.OrderId == orderId)
                .ToListAsync();
        }
    }
}