using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public class FabricIssueService : IFabricIssueService
    {
        private readonly AppDbContext _context;

        public FabricIssueService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FabricIssue>> GetAllAsync()
        {
            return await _context.FabricIssues
                .Include(f => f.Order)
                .Include(f => f.Item)
                .Include(f => f.Employee)
                .ToListAsync();
        }

        public async Task<FabricIssue?> GetByIdAsync(int id)
        {
            return await _context.FabricIssues
                .Include(f => f.Order)
                .Include(f => f.Item)
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FabricIssue> CreateAsync(FabricIssueViewModel model)
        {
            var issue = new FabricIssue
            {
                OrderId = model.OrderId,
                ItemId = model.ItemId,
                Quantity = model.Quantity,
                IssuedTo = model.IssuedTo,
                Date = model.Date,
                Status = model.Status
            };

            _context.FabricIssues.Add(issue);

            var item = await _context.Items.FindAsync(model.ItemId);
            if (item != null)
            {
                item.Stock -= model.Quantity;
            }

            await _context.SaveChangesAsync();
            return issue;
        }

        public async Task<FabricIssue> UpdateAsync(FabricIssueViewModel model)
        {
            var issue = await _context.FabricIssues.FindAsync(model.Id);
            if (issue == null)
                throw new KeyNotFoundException($"Fabric issue with ID {model.Id} not found.");

            var item = await _context.Items.FindAsync(issue.ItemId);
            if (item != null)
            {
                item.Stock += issue.Quantity;
            }

            issue.Quantity = model.Quantity;
            issue.IssuedTo = model.IssuedTo;
            issue.Status = model.Status;

            if (item != null)
            {
                item.Stock -= model.Quantity;
            }

            await _context.SaveChangesAsync();
            return issue;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var issue = await _context.FabricIssues.FindAsync(id);
            if (issue == null)
                return false;

            var item = await _context.Items.FindAsync(issue.ItemId);
            if (item != null)
            {
                item.Stock += issue.Quantity;
            }

            _context.FabricIssues.Remove(issue);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FabricIssue>> GetByOrderIdAsync(int orderId)
        {
            return await _context.FabricIssues
                .Include(f => f.Item)
                .Where(f => f.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FabricIssue>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.FabricIssues
                .Include(f => f.Order)
                .Where(f => f.IssuedTo == employeeId)
                .ToListAsync();
        }
    }
}