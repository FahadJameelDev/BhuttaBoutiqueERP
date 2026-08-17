using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using BhuttaBoutiqueERP.Models.Production.ViewModels;
using BhuttaBoutiqueERP.Services.Production;
using BhuttaBoutiqueERP.Services.Inventory;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Production
{
    [Route("Production/FabricAllocation")]
    public class FabricAllocationController : Controller
    {
        private readonly IFabricAllocationService _fabricAllocationService;
        private readonly IFabricDemandService _fabricDemandService;
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;

        public FabricAllocationController(
            IFabricAllocationService fabricAllocationService,
            IFabricDemandService fabricDemandService,
            IItemService itemService,
            IOrderService orderService)
        {
            _fabricAllocationService = fabricAllocationService;
            _fabricDemandService = fabricDemandService;
            _itemService = itemService;
            _orderService = orderService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var allocations = await _fabricAllocationService.GetAllAsync();
            return View("~/Views/Production/FabricAllocation/Index.cshtml", allocations);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var demands = await _fabricDemandService.GetAllAsync();
            var pendingDemands = demands.Where(d => d.Status == "Pending" || d.Status == "Allocated").ToList();

            ViewBag.Demands = pendingDemands;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricAllocation/Create.cshtml");
            }

            return View("~/Views/Production/FabricAllocation/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FabricAllocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Demands = await _fabricDemandService.GetAllAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/FabricAllocation/Create.cshtml", model);
                }
                return View("~/Views/Production/FabricAllocation/Create.cshtml", model);
            }

            try
            {
                await _fabricAllocationService.AllocateAsync(model);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Fabric allocated successfully!", redirectUrl = "/Production/FabricAllocation" });
                }

                return RedirectToAction(nameof(Index));
            }
            catch (System.InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Production/FabricAllocation/Create.cshtml", model);
                }
                return View("~/Views/Production/FabricAllocation/Create.cshtml", model);
            }
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var allocation = await _fabricAllocationService.GetByIdAsync(id);
            if (allocation == null) return NotFound();

            var model = new DemandAllocationViewModel
            {
                Id = allocation.Id,
                OrderId = allocation.OrderId,
                ItemId = allocation.ItemId,
                DemandQuantity = allocation.DemandQuantity,
                AllocatedQuantity = allocation.AllocatedQuantity,
                Status = allocation.Status,
                Date = allocation.Date
            };

            ViewBag.Orders = await _orderService.GetAllAsync();
            ViewBag.Items = await _itemService.GetAllAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Production/FabricAllocation/Edit.cshtml", model);
            }

            return View("~/Views/Production/FabricAllocation/Edit.cshtml", model);
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
                    return PartialView("~/Views/Production/FabricAllocation/Edit.cshtml", model);
                }
                return View("~/Views/Production/FabricAllocation/Edit.cshtml", model);
            }

            await _fabricAllocationService.UpdateAllocationAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Allocation updated successfully!", redirectUrl = "/Production/FabricAllocation" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Deallocate/{id:int}")]
        public async Task<IActionResult> Deallocate(int id)
        {
            await _fabricAllocationService.DeallocateAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Allocation deallocated successfully!", redirectUrl = "/Production/FabricAllocation" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("ByOrder/{orderId:int}")]
        public async Task<IActionResult> ByOrder(int orderId)
        {
            var allocations = await _fabricAllocationService.GetByOrderIdAsync(orderId);
            return View("~/Views/Production/FabricAllocation/Index.cshtml", allocations);
        }
    }
}