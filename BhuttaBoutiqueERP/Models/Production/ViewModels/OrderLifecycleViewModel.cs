using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Production.ViewModels
{
    public class OrderLifecycleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Stage is required")]
        [MaxLength(50, ErrorMessage = "Stage cannot exceed 50 characters")]
        public string Stage { get; set; } = "Cutting";

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        // ── For dropdowns & display ──
        public string? OrderNumber { get; set; }
        public string? EmployeeName { get; set; }
    }
}