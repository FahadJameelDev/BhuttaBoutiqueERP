using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Services.HR
{
    public class CommissionRateService : ICommissionRateService
    {
        private readonly AppDbContext _context;

        public CommissionRateService(AppDbContext context)
        {
            _context = context;
        }

        // 👇 UPDATE KI GAYI METHOD (Include add kar diya)
        public async Task<IEnumerable<CommissionRate>> GetAllAsync()
        {
            return await _context.CommissionRates
                .Include(c => c.ServiceCatalog) // Yeh line zaroori hai taake Service ka naam view mein aaye
                .ToListAsync();
        }

        public async Task<CommissionRate?> GetByIdAsync(int id)
        {
            // GetById mein bhi Include laga sakte hain agar Edit/Delete modal mein Service name dikhana ho
            return await _context.CommissionRates
                .Include(c => c.ServiceCatalog)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(CommissionRate commissionRate)
        {
            _context.CommissionRates.Add(commissionRate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CommissionRate commissionRate)
        {
            _context.CommissionRates.Update(commissionRate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var commissionRate = await _context.CommissionRates.FindAsync(id);
            if (commissionRate != null)
            {
                _context.CommissionRates.Remove(commissionRate);
                await _context.SaveChangesAsync();
            }
        }
    }
}