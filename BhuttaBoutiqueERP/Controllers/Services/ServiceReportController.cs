using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/Reports")]
    public class ServiceReportController : Controller
    {
        private readonly IServiceReportService _serviceReportService;

        public ServiceReportController(IServiceReportService serviceReportService)
        {
            _serviceReportService = serviceReportService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var model = await _serviceReportService.GetReportDataAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Reports/Index.cshtml", model);
            }

            return View("~/Views/Services/Reports/Index.cshtml", model);
        }
    }
}