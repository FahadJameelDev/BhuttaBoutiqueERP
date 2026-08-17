using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/Attendance/[action]")]
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("~/HR/Attendance")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View("~/Views/HR/Attendance/Index.cshtml", attendances);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            return PartialView("~/Views/HR/Attendance/_Create.cshtml", new Attendance());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Attendance recorded successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", attendance.EmployeeId);
            return PartialView("~/Views/HR/Attendance/_Create.cshtml", attendance);
        }

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return NotFound();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", attendance.EmployeeId);
            return PartialView("~/Views/HR/Attendance/_Edit.cshtml", attendance);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attendance attendance)
        {
            if (id != attendance.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(attendance);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Attendance updated successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", attendance.EmployeeId);
            return PartialView("~/Views/HR/Attendance/_Edit.cshtml", attendance);
        }

        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null) return NotFound();

            return PartialView("~/Views/HR/Attendance/_Delete.cshtml", attendance);
        }

        [HttpPost("Delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Attendance deleted successfully!", redirectUrl = Url.Action("Index") });
        }
    }
}