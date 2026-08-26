using Microsoft.AspNetCore.Mvc;
using BhuttaBoutiqueERP.Services.Reports;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    [Route("reports/hr")]
    public class HRController : Controller
    {
        private readonly IHRReportService _hrReportService;

        public HRController(IHRReportService hrReportService)
        {
            _hrReportService = hrReportService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = await _hrReportService.GetHRSummaryReportAsync();
            return View("~/Views/Reports/HR/Index.cshtml", model);
        }
    }
}