using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhuttaBoutiqueERP.Services.HR
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        // ✅ FIXED DELETE
        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                // 1. Saare child records pehle delete karo
                _context.Attendances.RemoveRange(_context.Attendances.Where(x => x.EmployeeId == id));
                _context.Leaves.RemoveRange(_context.Leaves.Where(x => x.EmployeeId == id));
                _context.Payrolls.RemoveRange(_context.Payrolls.Where(x => x.EmployeeId == id));
                _context.Advances.RemoveRange(_context.Advances.Where(x => x.EmployeeId == id));
                _context.FabricIssues.RemoveRange(_context.FabricIssues.Where(x => x.IssuedTo == id));
                _context.OrderLifecycles.RemoveRange(_context.OrderLifecycles.Where(x => x.EmployeeId == id));

                // 2. Ab Employee delete karo
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}