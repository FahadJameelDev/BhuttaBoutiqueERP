using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;
using BhuttaBoutiqueERP.Services.Services;

namespace BhuttaBoutiqueERP.Controllers.Services
{
    [Route("Services/DesignLibrary")]
    public class DesignLibraryController : Controller
    {
        private readonly IDesignLibraryService _designLibraryService;

        public DesignLibraryController(IDesignLibraryService designLibraryService)
        {
            _designLibraryService = designLibraryService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var designs = await _designLibraryService.GetAllAsync();
            return View("~/Views/Services/DesignLibrary/Index.cshtml", designs);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new DesignLibraryViewModel
            {
                Status = "Active",
                Complexity = "Medium"
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/DesignLibrary/Create.cshtml", model);
            }

            return View("~/Views/Services/DesignLibrary/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DesignLibraryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/DesignLibrary/Create.cshtml", model);
                }
                return View("~/Views/Services/DesignLibrary/Create.cshtml", model);
            }

            await _designLibraryService.CreateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Design added successfully!", redirectUrl = "/Services/DesignLibrary" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var design = await _designLibraryService.GetByIdAsync(id);
            if (design == null) return NotFound();

            var model = new DesignLibraryViewModel
            {
                Id = design.Id,
                Name = design.Name,
                Category = design.Category,
                Type = design.Type,
                Price = design.Price,
                Complexity = design.Complexity,
                Image = design.Image,
                Status = design.Status
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/DesignLibrary/Edit.cshtml", model);
            }

            return View("~/Views/Services/DesignLibrary/Edit.cshtml", model);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DesignLibraryViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Services/DesignLibrary/Edit.cshtml", model);
                }
                return View("~/Views/Services/DesignLibrary/Edit.cshtml", model);
            }

            await _designLibraryService.UpdateAsync(model);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Design updated successfully!", redirectUrl = "/Services/DesignLibrary" });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var design = await _designLibraryService.GetByIdAsync(id);
            if (design == null) return NotFound();

            // ✅ Convert to ViewModel for consistent view binding
            var model = new DesignLibraryViewModel
            {
                Id = design.Id,
                Name = design.Name,
                Category = design.Category,
                Type = design.Type,
                Price = design.Price,
                Complexity = design.Complexity,
                Image = design.Image,
                Status = design.Status
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Services/DesignLibrary/Delete.cshtml", model);
            }

            return View("~/Views/Services/DesignLibrary/Delete.cshtml", model);
        }

        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _designLibraryService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Design deleted successfully!", redirectUrl = "/Services/DesignLibrary" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}