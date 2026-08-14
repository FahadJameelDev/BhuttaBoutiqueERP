using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;

namespace BhuttaBoutiqueERP.Controllers.Inventory
{
    [Route("Inventory/StockIn")]
    public class StockInController : Controller
    {
        private readonly IStockInService _stockInService;
        private readonly IItemService _itemService;
        private readonly ISupplierService _supplierService;

        public StockInController(IStockInService stockInService, IItemService itemService, ISupplierService supplierService)
        {
            _stockInService = stockInService;
            _itemService = itemService;
            _supplierService = supplierService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var stockIns = await _stockInService.GetAllAsync();
            return View("~/Views/Inventory/StockIn/Index.cshtml", stockIns);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new StockInViewModel
            {
                Date = System.DateTime.Now,
                Status = "Pending"
            };

            ViewBag.Items = await _itemService.GetAllAsync();
            ViewBag.Suppliers = await _supplierService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/StockIn/Create.cshtml", model);
            }

            return View("~/Views/Inventory/StockIn/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Items = await _itemService.GetAllAsync();
                ViewBag.Suppliers = await _supplierService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/StockIn/Create.cshtml", model);
                }

                return View("~/Views/Inventory/StockIn/Create.cshtml", model);
            }

            await _stockInService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "StockIn") });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stockIn = await _stockInService.GetByIdAsync(id);
            if (stockIn == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/StockIn/Delete.cshtml", stockIn);
            }

            return View("~/Views/Inventory/StockIn/Delete.cshtml", stockIn);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stockInService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "StockIn") });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}