using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface IHRReportService
    {
        Task<HRReportVM> GetHRSummaryReportAsync();
    }
}