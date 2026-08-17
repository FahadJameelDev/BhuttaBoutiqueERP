using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Services.Production;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/DemandList")]
    public class DemandListController : Controller
    {
        private readonly IDemandListService _demandListService;

        public DemandListController(IDemandListService demandListService)
        {
            _demandListService = demandListService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var demands = await _demandListService.GetAllAsync();
            return View("~/Views/Production/DemandList/Index.cshtml", demands);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/DemandList/Create.cshtml");
            }

            return View("~/Views/Production/DemandList/Create.cshtml");
        }

        [HttpGet("Pending")]
        public async Task<IActionResult> Pending()
        {
            var demands = await _demandListService.GetPendingDemandsAsync();
            return View("~/Views/Production/DemandList/Index.cshtml", demands);
        }

        [HttpGet("ByOrder/{orderId:int}")]
        public async Task<IActionResult> ByOrder(int orderId)
        {
            var demands = await _demandListService.GetByOrderIdAsync(orderId);
            return View("~/Views/Production/DemandList/Index.cshtml", demands);
        }
    }
}