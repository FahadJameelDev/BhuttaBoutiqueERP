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
    public class OrderLifecycleService : IOrderLifecycleService
    {
        private readonly AppDbContext _context;

        public OrderLifecycleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderLifecycle>> GetAllAsync()
        {
            return await _context.OrderLifecycles
                .Include(o => o.Order)
                .Include(o => o.Employee)
                .ToListAsync();
        }

        public async Task<OrderLifecycle?> GetByIdAsync(int id)
        {
            return await _context.OrderLifecycles
                .Include(o => o.Order)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrderLifecycle> CreateAsync(OrderLifecycleViewModel model)
        {
            var lifecycle = new OrderLifecycle
            {
                OrderId = model.OrderId,
                EmployeeId = model.EmployeeId,
                Stage = model.Stage,
                Status = model.Status,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Notes = model.Notes
            };

            _context.OrderLifecycles.Add(lifecycle);
            await _context.SaveChangesAsync();
            return lifecycle;
        }

        public async Task<OrderLifecycle> UpdateAsync(OrderLifecycleViewModel model)
        {
            var lifecycle = await _context.OrderLifecycles.FindAsync(model.Id);
            if (lifecycle == null)
                throw new KeyNotFoundException($"Lifecycle with ID {model.Id} not found.");

            lifecycle.Stage = model.Stage;
            lifecycle.Status = model.Status;
            lifecycle.EndDate = model.EndDate;
            lifecycle.Notes = model.Notes;

            await _context.SaveChangesAsync();
            return lifecycle;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lifecycle = await _context.OrderLifecycles.FindAsync(id);
            if (lifecycle == null)
                return false;

            _context.OrderLifecycles.Remove(lifecycle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderLifecycle>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderLifecycles
                .Where(o => o.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderLifecycle>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.OrderLifecycles
                .Where(o => o.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderLifecycle>> GetByStageAsync(string stage)
        {
            return await _context.OrderLifecycles
                .Include(o => o.Order)
                .Where(o => o.Stage == stage)
                .ToListAsync();
        }
    }
}