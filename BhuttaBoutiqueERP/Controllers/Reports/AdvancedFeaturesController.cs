using Microsoft.AspNetCore.Mvc;
using BhuttaBoutiqueERP.Services.Reports;

namespace BhuttaBoutiqueERP.Controllers.Reports
{
    [Route("Reports/[controller]")]
    public class AdvancedFeaturesController : Controller
    {
        private readonly IActivityLogService _activityLogService;

        public AdvancedFeaturesController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var logs = await _activityLogService.GetAllLogsAsync();
            return View("~/Views/Reports/AdvancedFeatures/Index.cshtml", logs);
        }
    }
}