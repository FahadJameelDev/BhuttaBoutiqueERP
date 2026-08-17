using System;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IProductionReportService
    {
        Task<ProductionReportViewModel> GetReportDataAsync();
        Task<ProductionReportViewModel> GetReportByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}