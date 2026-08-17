using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Production.Entities;

namespace BhuttaBoutiqueERP.Services.Production
{
    public class DemandListService : IDemandListService
    {
        private readonly AppDbContext _context;

        public DemandListService(AppDbContext context)
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

        public async Task<IEnumerable<DemandAllocation>> GetByOrderIdAsync(int orderId)
        {
            return await _context.DemandAllocations
                .Include(d => d.Item)
                .Where(d => d.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DemandAllocation>> GetPendingDemandsAsync()
        {
            return await _context.DemandAllocations
                .Include(d => d.Order)
                .Include(d => d.Item)
                .Where(d => d.Status == "Pending" || d.Status == "Allocated")
                .ToListAsync();
        }
    }
}