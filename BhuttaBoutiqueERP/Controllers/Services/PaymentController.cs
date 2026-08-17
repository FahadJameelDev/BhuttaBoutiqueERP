using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.ViewModels;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/Payments")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllAsync();
            return View("~/Views/Services/Payments/Index.cshtml", payments);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new PaymentViewModel
            {
                Date = System.DateTime.Now,
                Status = "Completed",
                Method = "Cash"
            };

            ViewBag.Orders = await _orderService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Payments/Create.cshtml", model);
            }

            return View("~/Views/Services/Payments/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _orderService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/Payments/Create.cshtml", model);
                }
                return View("~/Views/Services/Payments/Create.cshtml", model);
            }

            await _paymentService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Payment recorded successfully!", redirectUrl = "/Services/Payments" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Payments/Delete.cshtml", payment);
            }

            return View("~/Views/Services/Payments/Delete.cshtml", payment);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Payment deleted successfully!", redirectUrl = "/Services/Payments" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}