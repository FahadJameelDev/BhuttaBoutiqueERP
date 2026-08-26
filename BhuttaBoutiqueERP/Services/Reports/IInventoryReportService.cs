using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface IInventoryReportService
    {
        Task<InventoryReportVM> GetInventorySummaryReportAsync();
    }
}