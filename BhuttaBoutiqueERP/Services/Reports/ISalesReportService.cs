using System;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public interface ISalesReportService
    {
        Task<SalesReportViewModel> GetSalesReportAsync(DateTime? startDate, DateTime? endDate, int? customerId, string? paymentStatus);
    }
}