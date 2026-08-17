using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.ViewModels;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/Catalog")]
    public class ServiceCatalogController : Controller
    {
        private readonly IServiceCatalogService _serviceCatalogService;

        public ServiceCatalogController(IServiceCatalogService serviceCatalogService)
        {
            _serviceCatalogService = serviceCatalogService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var services = await _serviceCatalogService.GetAllAsync();
            return View("~/Views/Services/ServiceCatalog/Index.cshtml", services);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new ServiceCatalogViewModel
            {
                Status = "Active",
                Complexity = "Medium"
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/ServiceCatalog/Create.cshtml", model);
            }

            return View("~/Views/Services/ServiceCatalog/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCatalogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/ServiceCatalog/Create.cshtml", model);
                }
                return View("~/Views/Services/ServiceCatalog/Create.cshtml", model);
            }

            await _serviceCatalogService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Service created successfully!", redirectUrl = "/Services/Catalog" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceCatalogService.GetByIdAsync(id);
            if (service == null) return NotFound();

            var model = new ServiceCatalogViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Category = service.Category,
                Price = service.Price,
                Time = service.Time,
                Complexity = service.Complexity,
                FabricRequire = service.FabricRequire,
                Status = service.Status
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/ServiceCatalog/Edit.cshtml", model);
            }

            return View("~/Views/Services/ServiceCatalog/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceCatalogViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/ServiceCatalog/Edit.cshtml", model);
                }
                return View("~/Views/Services/ServiceCatalog/Edit.cshtml", model);
            }

            await _serviceCatalogService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Service updated successfully!", redirectUrl = "/Services/Catalog" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceCatalogService.GetByIdAsync(id);
            if (service == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/ServiceCatalog/Delete.cshtml", service);
            }

            return View("~/Views/Services/ServiceCatalog/Delete.cshtml", service);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceCatalogService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Service deleted successfully!", redirectUrl = "/Services/Catalog" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}