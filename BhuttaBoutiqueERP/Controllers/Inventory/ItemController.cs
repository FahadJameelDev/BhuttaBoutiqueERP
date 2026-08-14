using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;

namespace BhuttaBoutiqueERP.Controllers.Inventory
{
    [Route("Inventory/Items")]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemService.GetAllAsync();
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Items/Index.cshtml", items);
            }
            return View("~/Views/Inventory/Items/Index.cshtml", items);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new ItemViewModel();
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Items/Create.cshtml", model);
            }
            return View("~/Views/Inventory/Items/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/Items/Create.cshtml", model);
                }
                return View("~/Views/Inventory/Items/Create.cshtml", model);
            }

            await _itemService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Item created successfully!", redirectUrl = "/Inventory/Items" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var model = new ItemViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Category = item.Category,
                Stock = item.Stock,
                Price = item.Price,
                Unit = item.Unit,
                ReorderLevel = item.ReorderLevel,
                Status = item.Status
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Items/Edit.cshtml", model);
            }

            return View("~/Views/Inventory/Items/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Inventory/Items/Edit.cshtml", model);
                }
                return View("~/Views/Inventory/Items/Edit.cshtml", model);
            }

            await _itemService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Item updated successfully!", redirectUrl = "/Inventory/Items" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Items/Delete.cshtml", item);
            }

            return View("~/Views/Inventory/Items/Delete.cshtml", item);
        }

        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _itemService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Item deleted successfully!", redirectUrl = "/Inventory/Items" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Inventory/LowStock")]
        public async Task<IActionResult> LowStock()
        {
            var items = await _itemService.GetAllAsync();
            var lowStockItems = items.Where(i => i.Stock <= i.ReorderLevel).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/LowStock/Index.cshtml", lowStockItems);
            }

            return View("~/Views/Inventory/LowStock/Index.cshtml", lowStockItems);
        }

        [HttpGet("/Inventory/Reports")]
        public async Task<IActionResult> Reports()
        {
            var items = await _itemService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Inventory/Reports/Index.cshtml", items);
            }

            return View("~/Views/Inventory/Reports/Index.cshtml", items);
        }
    }
}