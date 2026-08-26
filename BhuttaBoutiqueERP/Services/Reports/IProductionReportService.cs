using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface IProductionReportService
    {
        Task<ProductionReportVM> GetProductionSummaryReportAsync();
    }
}