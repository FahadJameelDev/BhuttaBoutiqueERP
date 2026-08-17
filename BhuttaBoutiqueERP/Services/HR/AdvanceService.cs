using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Services.HR
{
    public class AdvanceService : IAdvanceService
    {
        private readonly AppDbContext _context;

        public AdvanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Advance>> GetAllAsync()
        {
            return await _context.Advances
                .Include(adv => adv.Employee)
                .ToListAsync();
        }

        public async Task<Advance?> GetByIdAsync(int id)
        {
            return await _context.Advances
                .Include(adv => adv.Employee)
                .FirstOrDefaultAsync(adv => adv.Id == id);
        }

        public async Task AddAsync(Advance advance)
        {
            _context.Advances.Add(advance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Advance advance)
        {
            _context.Advances.Update(advance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var advance = await _context.Advances.FindAsync(id);
            if (advance != null)
            {
                _context.Advances.Remove(advance);
                await _context.SaveChangesAsync();
            }
        }
    }
}