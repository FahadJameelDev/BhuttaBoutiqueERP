using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;

namespace BhuttaBoutiqueERP.Controllers.Inventory
{
    [Route("Inventory/Wastage")]
    public class WastageController : Controller
    {
        private readonly IWastageService _wastageService;
        private readonly IItemService _itemService;

        public WastageController(IWastageService wastageService, IItemService itemService)
        {
            _wastageService = wastageService;
            _itemService = itemService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var wastages = await _wastageService.GetAllAsync();
            return View("~/Views/Inventory/Wastage/Index.cshtml", wastages);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new WastageViewModel
            {
                Date = System.DateTime.Now,
                Status = "Pending"
            };

            ViewBag.Items = await _itemService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Wastage/Create.cshtml", model);
            }

            return View("~/Views/Inventory/Wastage/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WastageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Items = await _itemService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/Wastage/Create.cshtml", model);
                }

                return View("~/Views/Inventory/Wastage/Create.cshtml", model);
            }

            await _wastageService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Wastage") });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var wastage = await _wastageService.GetByIdAsync(id);
            if (wastage == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Wastage/Delete.cshtml", wastage);
            }

            return View("~/Views/Inventory/Wastage/Delete.cshtml", wastage);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _wastageService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Wastage") });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}