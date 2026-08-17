using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/Leave/[action]")]
    public class LeaveController : Controller
    {
        private readonly AppDbContext _context;

        public LeaveController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("~/HR/Leave")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var leaves = await _context.Leaves
                .Include(l => l.Employee)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            return View("~/Views/HR/Leave/Index.cshtml", leaves);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            return PartialView("~/Views/HR/Leave/_Create.cshtml", new Leave());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Leave leave)
        {
            if (ModelState.IsValid)
            {
                _context.Leaves.Add(leave);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Leave request recorded successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", leave.EmployeeId);
            return PartialView("~/Views/HR/Leave/_Create.cshtml", leave);
        }

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return NotFound();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", leave.EmployeeId);
            return PartialView("~/Views/HR/Leave/_Edit.cshtml", leave);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Leave leave)
        {
            if (id != leave.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(leave);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Leave request updated successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", leave.EmployeeId);
            return PartialView("~/Views/HR/Leave/_Edit.cshtml", leave);
        }

        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leave == null) return NotFound();

            return PartialView("~/Views/HR/Leave/_Delete.cshtml", leave);
        }

        [HttpPost("Delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Leave request deleted successfully!", redirectUrl = Url.Action("Index") });
        }
    }
}