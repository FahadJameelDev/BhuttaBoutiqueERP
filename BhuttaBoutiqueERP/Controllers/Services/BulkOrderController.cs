using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.ViewModels;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/BulkOrders")]
    public class BulkOrderController : Controller
    {
        private readonly IBulkOrderService _bulkOrderService;
        private readonly ICustomerService _customerService;

        public BulkOrderController(IBulkOrderService bulkOrderService, ICustomerService customerService)
        {
            _bulkOrderService = bulkOrderService;
            _customerService = customerService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var bulkOrders = await _bulkOrderService.GetAllAsync();
            return View("~/Views/Services/BulkOrders/Index.cshtml", bulkOrders);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new BulkOrderViewModel
            {
                Status = "Pending",
                OrderDate = System.DateTime.Now,
                DeliveryDate = System.DateTime.Now.AddDays(14)
            };

            ViewBag.Customers = await _customerService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/BulkOrders/Create.cshtml", model);
            }

            return View("~/Views/Services/BulkOrders/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BulkOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/BulkOrders/Create.cshtml", model);
                }
                return View("~/Views/Services/BulkOrders/Create.cshtml", model);
            }

            await _bulkOrderService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Bulk order created successfully!", redirectUrl = "/Services/BulkOrders" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bulkOrder = await _bulkOrderService.GetByIdAsync(id);
            if (bulkOrder == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/BulkOrders/Delete.cshtml", bulkOrder);
            }

            return View("~/Views/Services/BulkOrders/Delete.cshtml", bulkOrder);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bulkOrderService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Bulk order deleted successfully!", redirectUrl = "/Services/BulkOrders" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}