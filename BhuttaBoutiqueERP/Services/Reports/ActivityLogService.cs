using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models;
using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly AppDbContext _context;

        public ActivityLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityLogVM>> GetAllLogsAsync()
        {
            return await _context.ActivityLogs
                .OrderByDescending(l => l.Timestamp)
                .Select(l => new ActivityLogVM
                {
                    Id = l.Id,
                    UserName = l.UserName,
                    ActionType = l.ActionType,
                    ModuleName = l.ModuleName,
                    Description = l.Description,
                    Timestamp = l.Timestamp,
                    IpAddress = l.IpAddress
                })
                .ToListAsync();
        }

        public async Task LogActivityAsync(string userName, string actionType, string moduleName, string description, string ipAddress)
        {
            var log = new ActivityLog
            {
                UserName = userName,
                ActionType = actionType,
                ModuleName = moduleName,
                Description = description,
                Timestamp = DateTime.Now,
                IpAddress = ipAddress
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}