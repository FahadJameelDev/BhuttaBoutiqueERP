using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhuttaBoutiqueERP.Models.HR.ViewModels
{
    public class PayrollViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee selection is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Base Salary")]
        public decimal BaseSalary { get; set; }

        public decimal Commission { get; set; } = 0;

        public decimal Overtime { get; set; } = 0;

        public decimal Bonus { get; set; } = 0;

        public decimal Deductions { get; set; } = 0;

        // Computed property — no manual calculation bugs
        [Display(Name = "Net Salary")]
        public decimal NetSalary => BaseSalary + Commission + Overtime + Bonus - Deductions;

        [Required, MaxLength(20)]
        public string Month { get; set; } = string.Empty; // e.g. "August-2026"

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Unpaid";

        public IEnumerable<SelectListItem>? EmployeeList { get; set; }
    }
}