using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface IFinancialReportService
    {
        Task<FinancialReportVM> GetFinancialSummaryReportAsync();
    }
}