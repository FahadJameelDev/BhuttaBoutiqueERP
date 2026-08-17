using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Production.ViewModels
{
    public class DemandAllocationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Item is required")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Demand quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Demand quantity must be greater than 0")]
        public decimal DemandQuantity { get; set; }

        [Required(ErrorMessage = "Allocated quantity is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Allocated quantity must be a positive value")]
        public decimal AllocatedQuantity { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        // ── For dropdowns & display ──
        public string? OrderNumber { get; set; }
        public string? ItemName { get; set; }
    }
}