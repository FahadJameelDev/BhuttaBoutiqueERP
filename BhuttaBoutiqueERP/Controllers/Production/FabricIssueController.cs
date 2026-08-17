using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.ViewModels;
using BhuttaBoutiqueERP.Services.Production;
using BhuttaBoutiqueERP.Services.Inventory;
using BhuttaBoutiqueERP.Services.Services;
using BhuttaBoutiqueERP.Services.Auth;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/FabricIssue")]
    public class FabricIssueController : Controller
    {
        private readonly IFabricIssueService _fabricIssueService;
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public FabricIssueController(
            IFabricIssueService fabricIssueService,
            IItemService itemService,
            IOrderService orderService,
            IUserService userService)
        {
            _fabricIssueService = fabricIssueService;
            _itemService = itemService;
            _orderService = orderService;
            _userService = userService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var issues = await _fabricIssueService.GetAllAsync();
            return View("~/Views/Production/FabricIssue/Index.cshtml", issues);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new FabricIssueViewModel
            {
                Date = System.DateTime.Now,
                Status = "Pending"
            };

            ViewBag.Items = await _itemService.GetAllAsync();
            ViewBag.Orders = await _orderService.GetAllAsync();
            ViewBag.Employees = await _userService.GetAllEmployeesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricIssue/Create.cshtml", model);
            }

            return View("~/Views/Production/FabricIssue/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FabricIssueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Items = await _itemService.GetAllAsync();
                ViewBag.Orders = await _orderService.GetAllAsync();
                ViewBag.Employees = await _userService.GetAllEmployeesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/FabricIssue/Create.cshtml", model);
                }
                return View("~/Views/Production/FabricIssue/Create.cshtml", model);
            }

            await _fabricIssueService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Fabric issued successfully!", redirectUrl = "/Production/FabricIssue" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var issue = await _fabricIssueService.GetByIdAsync(id);
            if (issue == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricIssue/Delete.cshtml", issue);
            }

            return View("~/Views/Production/FabricIssue/Delete.cshtml", issue);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _fabricIssueService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Fabric issue deleted successfully!", redirectUrl = "/Production/FabricIssue" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("ByOrder/{orderId:int}")]
        public async Task<IActionResult> ByOrder(int orderId)
        {
            var issues = await _fabricIssueService.GetByOrderIdAsync(orderId);
            return View("~/Views/Production/FabricIssue/Index.cshtml", issues);
        }

        [HttpGet("ByEmployee/{employeeId:int}")]
        public async Task<IActionResult> ByEmployee(int employeeId)
        {
            var issues = await _fabricIssueService.GetByEmployeeIdAsync(employeeId);
            return View("~/Views/Production/FabricIssue/Index.cshtml", issues);
        }
    }
}