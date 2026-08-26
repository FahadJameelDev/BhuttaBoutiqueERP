using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BhuttaBoutiqueERP.Services.Reports;
using BhuttaBoutiqueERP.Models.Reports;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    // Yeh attribute ensure karega ke /Reports/Sales par yeh chale
    [Route("Reports/Sales")]
    public class SalesController : Controller
    {
        private readonly ISalesReportService _salesReportService;

        public SalesController(ISalesReportService salesReportService)
        {
            _salesReportService = salesReportService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? customerId, string? paymentStatus)
        {
            var model = await _salesReportService.GetSalesReportAsync(startDate, endDate, customerId, paymentStatus);

            // Tumhari image ke mutabiq exact view path:
            return View("~/Views/Reports/Sales/Index.cshtml", model);
        }
    }
}