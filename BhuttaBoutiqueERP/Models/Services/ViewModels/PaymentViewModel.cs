using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [MaxLength(50, ErrorMessage = "Payment method cannot exceed 50 characters")]
        public string Method { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Completed";

        // For dropdowns & display
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
    }
}