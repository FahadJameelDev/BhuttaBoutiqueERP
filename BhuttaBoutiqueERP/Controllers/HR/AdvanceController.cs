using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/Advance/[action]")]
    public class AdvanceController : Controller
    {
        private readonly AppDbContext _context;

        public AdvanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("~/HR/Advance")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var advances = await _context.Advances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View("~/Views/HR/Advance/Index.cshtml", advances);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            return PartialView("~/Views/HR/Advance/_Create.cshtml", new Advance());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Advance advance)
        {
            if (ModelState.IsValid)
            {
                _context.Advances.Add(advance);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Advance record added successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", advance.EmployeeId);
            return PartialView("~/Views/HR/Advance/_Create.cshtml", advance);
        }

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var advance = await _context.Advances.FindAsync(id);
            if (advance == null) return NotFound();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", advance.EmployeeId);
            return PartialView("~/Views/HR/Advance/_Edit.cshtml", advance);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Advance advance)
        {
            if (id != advance.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(advance);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Advance record updated successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", advance.EmployeeId);
            return PartialView("~/Views/HR/Advance/_Edit.cshtml", advance);
        }

        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var advance = await _context.Advances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (advance == null) return NotFound();

            return PartialView("~/Views/HR/Advance/_Delete.cshtml", advance);
        }

        [HttpPost("Delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advance = await _context.Advances.FindAsync(id);
            if (advance != null)
            {
                _context.Advances.Remove(advance);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Advance record deleted successfully!", redirectUrl = Url.Action("Index") });
        }
    }
}