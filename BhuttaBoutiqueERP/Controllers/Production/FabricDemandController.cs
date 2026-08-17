using BhuttaBoutiqueERP.Models.Production.ViewModels;
using BhuttaBoutiqueERP.Services.Inventory;
using BhuttaBoutiqueERP.Services.Production;
using BhuttaBoutiqueERP.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/FabricDemand")]
    public class FabricDemandController : Controller
    {
        private readonly IFabricDemandService _fabricDemandService;
        private readonly IOrderService _orderService;
        private readonly IItemService _itemService;

        public FabricDemandController(IFabricDemandService fabricDemandService, IOrderService orderService, IItemService itemService)
        {
            _fabricDemandService = fabricDemandService;
            _orderService = orderService;
            _itemService = itemService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var demands = await _fabricDemandService.GetAllAsync();
            return View("~/Views/Production/FabricDemand/Index.cshtml", demands);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new DemandAllocationViewModel
            {
                Status = "Pending",
                Date = System.DateTime.Now
            };

            ViewBag.Orders = await _orderService.GetAllAsync();
            ViewBag.Items = await _itemService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricDemand/Create.cshtml", model);
            }

            return View("~/Views/Production/FabricDemand/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DemandAllocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _orderService.GetAllAsync();
                ViewBag.Items = await _itemService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/FabricDemand/Create.cshtml", model);
                }
                return View("~/Views/Production/FabricDemand/Create.cshtml", model);
            }

            await _fabricDemandService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Fabric demand created successfully!", redirectUrl = "/Production/FabricDemand" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var demand = await _fabricDemandService.GetByIdAsync(id);
            if (demand == null) return NotFound();

            var model = new DemandAllocationViewModel
            {
                Id = demand.Id,
                OrderId = demand.OrderId,
                ItemId = demand.ItemId,
                DemandQuantity = demand.DemandQuantity,
                AllocatedQuantity = demand.AllocatedQuantity,
                Status = demand.Status,
                Date = demand.Date
            };

            ViewBag.Orders = await _orderService.GetAllAsync();
            ViewBag.Items = await _itemService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricDemand/Edit.cshtml", model);
            }

            return View("~/Views/Production/FabricDemand/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DemandAllocationViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _orderService.GetAllAsync();
                ViewBag.Items = await _itemService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/FabricDemand/Edit.cshtml", model);
                }
                return View("~/Views/Production/FabricDemand/Edit.cshtml", model);
            }

            await _fabricDemandService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Fabric demand updated successfully!", redirectUrl = "/Production/FabricDemand" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var demand = await _fabricDemandService.GetByIdAsync(id);
            if (demand == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricDemand/Delete.cshtml", demand);
            }

            return View("~/Views/Production/FabricDemand/Delete.cshtml", demand);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _fabricDemandService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Fabric demand deleted successfully!", redirectUrl = "/Production/FabricDemand" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}