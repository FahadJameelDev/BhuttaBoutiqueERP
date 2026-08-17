using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.ViewModels;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/Measurements")]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementService _measurementService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public MeasurementController(IMeasurementService measurementService, ICustomerService customerService, IOrderService orderService)
        {
            _measurementService = measurementService;
            _customerService = customerService;
            _orderService = orderService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var measurements = await _measurementService.GetAllAsync();
            return View("~/Views/Services/Measurements/Index.cshtml", measurements);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new MeasurementViewModel
            {
                Date = System.DateTime.Now
            };

            ViewBag.Customers = await _customerService.GetAllAsync();
            ViewBag.Orders = await _orderService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Measurements/Create.cshtml", model);
            }

            return View("~/Views/Services/Measurements/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeasurementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerService.GetAllAsync();
                ViewBag.Orders = await _orderService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/Measurements/Create.cshtml", model);
                }
                return View("~/Views/Services/Measurements/Create.cshtml", model);
            }

            await _measurementService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Measurements recorded successfully!", redirectUrl = "/Services/Measurements" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var measurement = await _measurementService.GetByIdAsync(id);
            if (measurement == null) return NotFound();

            var model = new MeasurementViewModel
            {
                Id = measurement.Id,
                CustomerId = measurement.CustomerId,
                OrderId = measurement.OrderId,
                Chest = measurement.Chest,
                Waist = measurement.Waist,
                Hips = measurement.Hips,
                Shoulder = measurement.Shoulder,
                Armhole = measurement.Armhole,
                Neck = measurement.Neck,
                KurtaLength = measurement.KurtaLength,
                ShalwarLength = measurement.ShalwarLength,
                Paicha = measurement.Paicha,
                SleeveLength = measurement.SleeveLength,
                Bust = measurement.Bust,
                KameezLength = measurement.KameezLength,
                DupattaLength = measurement.DupattaLength,
                Damman = measurement.Damman,
                Date = measurement.Date,
                Notes = measurement.Notes
            };

            ViewBag.Customers = await _customerService.GetAllAsync();
            ViewBag.Orders = await _orderService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Measurements/Edit.cshtml", model);
            }

            return View("~/Views/Services/Measurements/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MeasurementViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerService.GetAllAsync();
                ViewBag.Orders = await _orderService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/Measurements/Edit.cshtml", model);
                }
                return View("~/Views/Services/Measurements/Edit.cshtml", model);
            }

            await _measurementService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Measurements updated successfully!", redirectUrl = "/Services/Measurements" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var measurement = await _measurementService.GetByIdAsync(id);
            if (measurement == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/Measurements/Delete.cshtml", measurement);
            }

            return View("~/Views/Services/Measurements/Delete.cshtml", measurement);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _measurementService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Measurement record deleted successfully!", redirectUrl = "/Services/Measurements" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}