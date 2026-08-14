using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;

namespace BhuttaBoutiqueERP.Controllers.Inventory
{
    [Route("Inventory/Suppliers")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return View("~/Views/Inventory/Suppliers/Index.cshtml", suppliers);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new SupplierViewModel
            {
                Status = "Active"
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Suppliers/Create.cshtml", model);
            }

            return View("~/Views/Inventory/Suppliers/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/Suppliers/Create.cshtml", model);
                }
                return View("~/Views/Inventory/Suppliers/Create.cshtml", model);
            }

            await _supplierService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Supplier") });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null) return NotFound();

            var model = new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Contact = supplier.Contact,
                Email = supplier.Email,
                Address = supplier.Address,
                Status = supplier.Status
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Suppliers/Edit.cshtml", model);
            }

            return View("~/Views/Inventory/Suppliers/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/Suppliers/Edit.cshtml", model);
                }
                return View("~/Views/Inventory/Suppliers/Edit.cshtml", model);
            }

            await _supplierService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Supplier") });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Suppliers/Delete.cshtml", supplier);
            }

            return View("~/Views/Inventory/Suppliers/Delete.cshtml", supplier);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _supplierService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Supplier") });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}