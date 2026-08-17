using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.HR.Entities;
using BhuttaBoutiqueERP.Services.HR;
using BhuttaBoutiqueERP.Services.Services; // ServiceCatalog ke liye

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/CommissionRates")]
    public class CommissionRateController : Controller
    {
        private readonly ICommissionRateService _commissionRateService;
        private readonly IServiceCatalogService _serviceCatalogService;

        public CommissionRateController(ICommissionRateService commissionRateService, IServiceCatalogService serviceCatalogService)
        {
            _commissionRateService = commissionRateService;
            _serviceCatalogService = serviceCatalogService;
        }

        // GET: /HR/CommissionRates
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var rates = await _commissionRateService.GetAllAsync();
            return View("~/Views/HR/CommissionRates/Index.cshtml", rates);
        }

        // GET: /HR/CommissionRates/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Services = await _serviceCatalogService.GetAllAsync();
            var model = new CommissionRate();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/CommissionRates/Create.cshtml", model);
            }
            return View("~/Views/HR/CommissionRates/Create.cshtml", model);
        }

        // POST: /HR/CommissionRates/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommissionRate commissionRate)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = await _serviceCatalogService.GetAllAsync();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/HR/CommissionRates/Create.cshtml", commissionRate);
                }
                return View("~/Views/HR/CommissionRates/Create.cshtml", commissionRate);
            }

            await _commissionRateService.AddAsync(commissionRate);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Commission rate added successfully!", redirectUrl = "/HR/CommissionRates" });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /HR/CommissionRates/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var commissionRate = await _commissionRateService.GetByIdAsync(id);
            if (commissionRate == null) return NotFound();

            ViewBag.Services = await _serviceCatalogService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/CommissionRates/Edit.cshtml", commissionRate);
            }
            return View("~/Views/HR/CommissionRates/Edit.cshtml", commissionRate);
        }

        // POST: /HR/CommissionRates/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CommissionRate commissionRate)
        {
            if (id != commissionRate.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Services = await _serviceCatalogService.GetAllAsync();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/HR/CommissionRates/Edit.cshtml", commissionRate);
                }
                return View("~/Views/HR/CommissionRates/Edit.cshtml", commissionRate);
            }

            await _commissionRateService.UpdateAsync(commissionRate);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Commission rate updated successfully!", redirectUrl = "/HR/CommissionRates" });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /HR/CommissionRates/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var commissionRate = await _commissionRateService.GetByIdAsync(id);
            if (commissionRate == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/CommissionRates/Delete.cshtml", commissionRate);
            }
            return View("~/Views/HR/CommissionRates/Delete.cshtml", commissionRate);
        }

        // POST: /HR/CommissionRates/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commissionRateService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Commission rate deleted successfully!", redirectUrl = "/HR/CommissionRates" });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}