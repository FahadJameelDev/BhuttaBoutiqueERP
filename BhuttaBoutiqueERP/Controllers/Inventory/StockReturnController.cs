using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;

namespace BhuttaBoutiqueERP.Controllers.Inventory
{
    [Route("Inventory/StockReturn")]
    public class StockReturnController : Controller
    {
        private readonly IStockReturnService _stockReturnService;
        private readonly IItemService _itemService;
        private readonly IStockOutService _stockOutService;

        public StockReturnController(IStockReturnService stockReturnService, IItemService itemService, IStockOutService stockOutService)
        {
            _stockReturnService = stockReturnService;
            _itemService = itemService;
            _stockOutService = stockOutService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var stockReturns = await _stockReturnService.GetAllAsync();
            return View("~/Views/Inventory/StockReturn/Index.cshtml", stockReturns);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new StockReturnViewModel
            {
                Date = System.DateTime.Now,
                Status = "Pending"
            };

            ViewBag.Items = await _itemService.GetAllAsync();
            ViewBag.StockOuts = await _stockOutService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/StockReturn/Create.cshtml", model);
            }

            return View("~/Views/Inventory/StockReturn/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Items = await _itemService.GetAllAsync();
                ViewBag.StockOuts = await _stockOutService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/StockReturn/Create.cshtml", model);
                }

                return View("~/Views/Inventory/StockReturn/Create.cshtml", model);
            }

            await _stockReturnService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "StockReturn") });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stockReturn = await _stockReturnService.GetByIdAsync(id);
            if (stockReturn == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/StockReturn/Delete.cshtml", stockReturn);
            }

            return View("~/Views/Inventory/StockReturn/Delete.cshtml", stockReturn);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stockReturnService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "StockReturn") });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}