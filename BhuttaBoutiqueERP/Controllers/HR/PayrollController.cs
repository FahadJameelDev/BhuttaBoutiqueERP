using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/Payroll/[action]")]
    public class PayrollController : Controller
    {
        private readonly AppDbContext _context;

        public PayrollController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("~/HR/Payroll")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var payrolls = await _context.Payrolls
                .Include(p => p.Employee)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View("~/Views/HR/Payroll/Index.cshtml", payrolls);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            return PartialView("~/Views/HR/Payroll/_Create.cshtml", new Payroll());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                // Calculate Net Salary automatically
                payroll.NetSalary = (payroll.BaseSalary + payroll.Commission + payroll.Overtime + payroll.Bonus) - payroll.Deductions;

                _context.Payrolls.Add(payroll);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Payroll generated successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", payroll.EmployeeId);
            return PartialView("~/Views/HR/Payroll/_Create.cshtml", payroll);
        }

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll == null) return NotFound();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", payroll.EmployeeId);
            return PartialView("~/Views/HR/Payroll/_Edit.cshtml", payroll);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payroll payroll)
        {
            if (id != payroll.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Recalculate Net Salary on update
                payroll.NetSalary = (payroll.BaseSalary + payroll.Commission + payroll.Overtime + payroll.Bonus) - payroll.Deductions;

                _context.Update(payroll);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Payroll updated successfully!", redirectUrl = Url.Action("Index") });
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", payroll.EmployeeId);
            return PartialView("~/Views/HR/Payroll/_Edit.cshtml", payroll);
        }

        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Request.Headers.ContainsKey("X-Requested-With") || Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return BadRequest();
            }

            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (payroll == null) return NotFound();

            return PartialView("~/Views/HR/Payroll/_Delete.cshtml", payroll);
        }

        [HttpPost("Delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Payroll deleted successfully!", redirectUrl = Url.Action("Index") });
        }
    }
}