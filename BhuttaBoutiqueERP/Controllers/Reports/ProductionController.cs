using Microsoft.AspNetCore.Mvc;
using BhuttaBoutiqueERP.Services.Reports;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    [Route("reports/production")]
    public class ProductionController : Controller
    {
        private readonly IProductionReportService _productionReportService;

        public ProductionController(IProductionReportService productionReportService)
        {
            _productionReportService = productionReportService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = await _productionReportService.GetProductionSummaryReportAsync();
            return View("~/Views/Reports/Production/Index.cshtml", model);
        }
    }
}