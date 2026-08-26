using Microsoft.AspNetCore.Mvc;
using BhuttaBoutiqueERP.Services.Reports;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    [Route("Reports/[controller]")]
    public class FinancialController : Controller
    {
        private readonly IFinancialReportService _financialReportService;

        public FinancialController(IFinancialReportService financialReportService)
        {
            _financialReportService = financialReportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reportData = await _financialReportService.GetFinancialSummaryReportAsync();
            return View("~/Views/Reports/Financial/Index.cshtml", reportData);
        }
    }
}