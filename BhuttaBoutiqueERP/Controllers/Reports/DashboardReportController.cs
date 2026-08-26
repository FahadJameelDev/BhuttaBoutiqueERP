using BhuttaBoutiqueERP.Services.Reports;
using BhuttaBoutiqueERP.Models.Reports.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    [Route("Reports/Dashboard")]
    public class DashboardReportController : Controller
    {
        private readonly IDashboardReportService _dashboardService;

        public DashboardReportController(IDashboardReportService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.GetDashboardOverviewAsync();

            // Partial view logic removed completely — direct full view return
            return View("~/Views/Reports/Dashboard/Index.cshtml", model);
        }
    }
}