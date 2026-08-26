using BhuttaBoutiqueERP.Models.Reports.ViewModels;
using System.Threading.Tasks;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface IDashboardReportService
    {
        Task<DashboardOverviewViewModel> GetDashboardOverviewAsync();
    }
}