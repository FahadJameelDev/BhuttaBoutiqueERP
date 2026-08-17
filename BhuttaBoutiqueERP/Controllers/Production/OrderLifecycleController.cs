using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.ViewModels;
using BhuttaBoutiqueERP.Services.Production;
using BhuttaBoutiqueERP.Services.Services;
using BhuttaBoutiqueERP.Services.Auth;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/OrderLifecycle")]
    public class OrderLifecycleController : Controller
    {
        private readonly IOrderLifecycleService _orderLifecycleService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderLifecycleController(
            IOrderLifecycleService orderLifecycleService,
            IOrderService orderService,
            IUserService userService)
        {
            _orderLifecycleService = orderLifecycleService;
            _orderService = orderService;
            _userService = userService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var lifecycles = await _orderLifecycleService.GetAllAsync();
            return View("~/Views/Production/OrderLifecycle/Index.cshtml", lifecycles);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new OrderLifecycleViewModel
            {
                Status = "Pending",
                StartDate = System.DateTime.Now,
                Stage = "Cutting"
            };

            ViewBag.Orders = await _orderService.GetAllAsync();
            ViewBag.Employees = await _userService.GetAllEmployeesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/OrderLifecycle/Create.cshtml", model);
            }

            return View("~/Views/Production/OrderLifecycle/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderLifecycleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _orderService.GetAllAsync();
                ViewBag.Employees = await _userService.GetAllEmployeesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/OrderLifecycle/Create.cshtml", model);
                }
                return View("~/Views/Production/OrderLifecycle/Create.cshtml", model);
            }

            await _orderLifecycleService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order lifecycle created successfully!", redirectUrl = "/Production/OrderLifecycle" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var lifecycle = await _orderLifecycleService.GetByIdAsync(id);
            if (lifecycle == null) return NotFound();

            var model = new OrderLifecycleViewModel
            {
                Id = lifecycle.Id,
                OrderId = lifecycle.OrderId,
                EmployeeId = lifecycle.EmployeeId,
                Stage = lifecycle.Stage,
                Status = lifecycle.Status,
                StartDate = lifecycle.StartDate,
                EndDate = lifecycle.EndDate,
                Notes = lifecycle.Notes
            };

            ViewBag.Orders = await _orderService.GetAllAsync();
            ViewBag.Employees = await _userService.GetAllEmployeesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/OrderLifecycle/Edit.cshtml", model);
            }

            return View("~/Views/Production/OrderLifecycle/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderLifecycleViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _orderService.GetAllAsync();
                ViewBag.Employees = await _userService.GetAllEmployeesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/OrderLifecycle/Edit.cshtml", model);
                }
                return View("~/Views/Production/OrderLifecycle/Edit.cshtml", model);
            }

            await _orderLifecycleService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order lifecycle updated successfully!", redirectUrl = "/Production/OrderLifecycle" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lifecycle = await _orderLifecycleService.GetByIdAsync(id);
            if (lifecycle == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/OrderLifecycle/Delete.cshtml", lifecycle);
            }

            return View("~/Views/Production/OrderLifecycle/Delete.cshtml", lifecycle);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderLifecycleService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order lifecycle deleted successfully!", redirectUrl = "/Production/OrderLifecycle" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("ByStage/{stage}")]
        public async Task<IActionResult> ByStage(string stage)
        {
            var lifecycles = await _orderLifecycleService.GetByStageAsync(stage);
            return View("~/Views/Production/OrderLifecycle/Index.cshtml", lifecycles);
        }

        [HttpGet("ByOrder/{orderId:int}")]
        public async Task<IActionResult> ByOrder(int orderId)
        {
            var lifecycles = await _orderLifecycleService.GetByOrderIdAsync(orderId);
            return View("~/Views/Production/OrderLifecycle/Index.cshtml", lifecycles);
        }

        [HttpGet("ByEmployee/{employeeId:int}")]
        public async Task<IActionResult> ByEmployee(int employeeId)
        {
            var lifecycles = await _orderLifecycleService.GetByEmployeeIdAsync(employeeId);
            return View("~/Views/Production/OrderLifecycle/Index.cshtml", lifecycles);
        }

        [HttpGet("Complete/{id:int}")]
        public async Task<IActionResult> Complete(int id)
        {
            var lifecycle = await _orderLifecycleService.GetByIdAsync(id);
            if (lifecycle == null) return NotFound();

            lifecycle.Status = "Completed";
            lifecycle.EndDate = System.DateTime.Now;

            await _orderLifecycleService.UpdateAsync(new OrderLifecycleViewModel
            {
                Id = lifecycle.Id,
                OrderId = lifecycle.OrderId,
                EmployeeId = lifecycle.EmployeeId,
                Stage = lifecycle.Stage,
                Status = lifecycle.Status,
                StartDate = lifecycle.StartDate,
                EndDate = lifecycle.EndDate,
                Notes = lifecycle.Notes
            });

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Stage completed successfully!", redirectUrl = "/Production/OrderLifecycle" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}