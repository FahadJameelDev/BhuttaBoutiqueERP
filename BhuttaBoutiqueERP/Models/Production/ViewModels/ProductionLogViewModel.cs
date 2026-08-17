using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Production.ViewModels
{
    public class ProductionLogViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Stage is required")]
        [MaxLength(50, ErrorMessage = "Stage cannot exceed 50 characters")]
        public string Stage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        // ── For dropdowns & display ──
        public string? OrderNumber { get; set; }
    }
}