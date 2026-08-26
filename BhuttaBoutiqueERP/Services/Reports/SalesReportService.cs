using System;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class SalesReportService : ISalesReportService
    {
        public async Task<SalesReportViewModel> GetSalesReportAsync(DateTime? startDate, DateTime? endDate, int? customerId, string? paymentStatus)
        {
            // Temporary dummy return taake build pass ho jaye
            return await Task.FromResult(new SalesReportViewModel());
        }
    }
}