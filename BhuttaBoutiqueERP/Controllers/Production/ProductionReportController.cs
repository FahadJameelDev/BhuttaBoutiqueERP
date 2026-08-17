using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.ViewModels;
using BhuttaBoutiqueERP.Services.Production;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/ProductionReports")]
    public class ProductionReportController : Controller
    {
        private readonly IProductionReportService _productionReportService;

        public ProductionReportController(IProductionReportService productionReportService)
        {
            _productionReportService = productionReportService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var model = await _productionReportService.GetReportDataAsync();
            return View("~/Views/Production/ProductionReports/Index.cshtml", model);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(DateTime? startDate, DateTime? endDate)
        {
            var model = new ProductionReportViewModel();

            if (startDate.HasValue && endDate.HasValue)
            {
                model = await _productionReportService.GetReportByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                model = await _productionReportService.GetReportDataAsync();
                model.ReportPeriod = "All Time";
            }

            return View("~/Views/Production/ProductionReports/Details.cshtml", model);
        }

        [HttpGet("ByDateRange")]
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var model = await _productionReportService.GetReportByDateRangeAsync(startDate, endDate);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/ProductionReports/Details.cshtml", model);
            }

            return View("~/Views/Production/ProductionReports/Details.cshtml", model);
        }
    }
}