using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.HR.Entities;
using BhuttaBoutiqueERP.Services.HR;

namespace BhuttaBoutiqueERP.Controllers.HR
{
    [Route("HR/Employees")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: /HR/Employees
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View("~/Views/HR/Employees/Index.cshtml", employees);
        }

        // GET: /HR/Employees/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new Employee { JoiningDate = System.DateTime.Now };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/Employees/Create.cshtml", model);
            }
            return View("~/Views/HR/Employees/Create.cshtml", model);
        }

        // POST: /HR/Employees/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/HR/Employees/Create.cshtml", employee);
                }
                return View("~/Views/HR/Employees/Create.cshtml", employee);
            }

            await _employeeService.AddAsync(employee);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Employee added successfully!", redirectUrl = "/HR/Employees" });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /HR/Employees/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/Employees/Edit.cshtml", employee);
            }
            return View("~/Views/HR/Employees/Edit.cshtml", employee);
        }

        // POST: /HR/Employees/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/HR/Employees/Edit.cshtml", employee);
                }
                return View("~/Views/HR/Employees/Edit.cshtml", employee);
            }

            await _employeeService.UpdateAsync(employee);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Employee updated successfully!", redirectUrl = "/HR/Employees" });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /HR/Employees/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/HR/Employees/Delete.cshtml", employee);
            }
            return View("~/Views/HR/Employees/Delete.cshtml", employee);
        }

        // POST: /HR/Employees/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Employee deleted successfully!", redirectUrl = "/HR/Employees" });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}