using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Services.HR
{
    public class PayrollService : IPayrollService
    {
        private readonly AppDbContext _context;

        public PayrollService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payroll>> GetAllAsync()
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .ToListAsync();
        }

        public async Task<Payroll?> GetByIdAsync(int id)
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Payroll payroll)
        {
            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payroll payroll)
        {
            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
            }
        }
    }
}