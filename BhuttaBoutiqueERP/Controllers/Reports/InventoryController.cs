using Microsoft.AspNetCore.Mvc;
using BhuttaBoutiqueERP.Services.Reports;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    [Route("reports/inventory")]
    public class InventoryController : Controller
    {
        private readonly IInventoryReportService _inventoryReportService;

        public InventoryController(IInventoryReportService inventoryReportService)
        {
            _inventoryReportService = inventoryReportService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = await _inventoryReportService.GetInventorySummaryReportAsync();
            return View("~/Views/Reports/Inventory/Index.cshtml", model);
        }
    }
}