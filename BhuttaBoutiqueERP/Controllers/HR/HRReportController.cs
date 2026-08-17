using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/Reports/[action]")]
    public class HRReportController : Controller
    {
        private readonly AppDbContext _context;

        public HRReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("~/HR/Reports")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // Fetch summary stats or available report types for HR dashboard
            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
            ViewBag.ActiveEmployees = await _context.Employees.CountAsync(e => e.Status == "Active");
            ViewBag.TotalAttendances = await _context.Attendances.CountAsync();
            ViewBag.TotalLeaves = await _context.Leaves.CountAsync();

            return View("~/Views/HR/Reports/Index.cshtml");
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(string reportType, string month)
        {
            object reportData = null;
            ViewBag.ReportTitle = reportType switch
            {
                "Attendance" => "Attendance Summary Report",
                "Payroll" => "Payroll Ledger Report",
                "Leave" => "Leave Tracking Report",
                "Advance" => "Advance & Loan Ledger Report",
                _ => "General HR Report"
            };

            ViewBag.ReportType = reportType;
            ViewBag.SelectedMonth = month;

            if (reportType == "Attendance")
            {
                reportData = await _context.Attendances
                    .Include(a => a.Employee)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync();
            }
            else if (reportType == "Payroll")
            {
                reportData = await _context.Payrolls
                    .Include(p => p.Employee)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();
            }
            else if (reportType == "Leave")
            {
                reportData = await _context.Leaves
                    .Include(l => l.Employee)
                    .OrderByDescending(l => l.StartDate)
                    .ToListAsync();
            }
            else if (reportType == "Advance")
            {
                reportData = await _context.Advances
                    .Include(a => a.Employee)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync();
            }
            else
            {
                reportData = await _context.Employees.ToListAsync();
            }

            // Check for AJAX request if opened via modal/dynamic loader
            if (Request.Headers.ContainsKey("X-Requested-With") && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/Reports/_ReportDetailsContent.cshtml", reportData);
            }

            return View("~/Views/HR/Reports/Details.cshtml", reportData);
        }
    }
}