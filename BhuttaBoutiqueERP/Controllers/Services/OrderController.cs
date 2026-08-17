using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.ViewModels;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/Orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IServiceCatalogService _serviceCatalogService;

        public OrderController(IOrderService orderService, ICustomerService customerService, IServiceCatalogService serviceCatalogService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _serviceCatalogService = serviceCatalogService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            return View("~/Views/Services/Orders/Index.cshtml", orders);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new OrderViewModel
            {
                Status = "Pending",
                OrderDate = System.DateTime.Now,
                DeliveryDate = System.DateTime.Now.AddDays(7)
            };

            ViewBag.Customers = await _customerService.GetAllAsync();
            ViewBag.Services = await _serviceCatalogService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Orders/Create.cshtml", model);
            }

            return View("~/Views/Services/Orders/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerService.GetAllAsync();
                ViewBag.Services = await _serviceCatalogService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/Orders/Create.cshtml", model);
                }
                return View("~/Views/Services/Orders/Create.cshtml", model);
            }

            await _orderService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order created successfully!", redirectUrl = "/Services/Orders" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();

            var model = new OrderViewModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                ServiceId = order.ServiceId,
                Total = order.Total,
                Advance = order.Advance,
                Remaining = order.Remaining,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate
            };

            ViewBag.Customers = await _customerService.GetAllAsync();
            ViewBag.Services = await _serviceCatalogService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Orders/Edit.cshtml", model);
            }

            return View("~/Views/Services/Orders/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerService.GetAllAsync();
                ViewBag.Services = await _serviceCatalogService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/Orders/Edit.cshtml", model);
                }
                return View("~/Views/Services/Orders/Edit.cshtml", model);
            }

            await _orderService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order updated successfully!", redirectUrl = "/Services/Orders" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Orders/Delete.cshtml", order);
            }

            return View("~/Views/Services/Orders/Delete.cshtml", order);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Order deleted successfully!", redirectUrl = "/Services/Orders" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}