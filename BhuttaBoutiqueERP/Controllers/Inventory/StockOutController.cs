using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;

namespace BhuttaBoutiqueERP.Controllers.Inventory
{
    [Route("Inventory/StockOut")]
    public class StockOutController : Controller
    {
        private readonly IStockOutService _stockOutService;
        private readonly IItemService _itemService;

        public StockOutController(IStockOutService stockOutService, IItemService itemService)
        {
            _stockOutService = stockOutService;
            _itemService = itemService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var stockOuts = await _stockOutService.GetAllAsync();
            return View("~/Views/Inventory/StockOut/Index.cshtml", stockOuts);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new StockOutViewModel
            {
                Date = System.DateTime.Now,
                Status = "Pending"
            };

            ViewBag.Items = await _itemService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/StockOut/Create.cshtml", model);
            }

            return View("~/Views/Inventory/StockOut/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockOutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Items = await _itemService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/StockOut/Create.cshtml", model);
                }

                return View("~/Views/Inventory/StockOut/Create.cshtml", model);
            }

            await _stockOutService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "StockOut") });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stockOut = await _stockOutService.GetByIdAsync(id);
            if (stockOut == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/StockOut/Delete.cshtml", stockOut);
            }

            return View("~/Views/Inventory/StockOut/Delete.cshtml", stockOut);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stockOutService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "StockOut") });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}