using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface IActivityLogService
    {
        Task<List<ActivityLogVM>> GetAllLogsAsync();
        Task LogActivityAsync(string userName, string actionType, string moduleName, string description, string ipAddress);
    }
}