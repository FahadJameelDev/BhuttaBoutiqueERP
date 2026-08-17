using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order number is required")]
        [MaxLength(50, ErrorMessage = "Order number cannot exceed 50 characters")]
        public string OrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Service is required")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Total is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Advance is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Advance must be a positive value")]
        public decimal Advance { get; set; }

        [Required(ErrorMessage = "Remaining is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Remaining must be a positive value")]
        public decimal Remaining { get; set; }

        [MaxLength(50, ErrorMessage = "Payment method cannot exceed 50 characters")]
        public string? PaymentMethod { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Delivery date is required")]
        public DateTime DeliveryDate { get; set; }

        // For dropdowns & display
        public string? CustomerName { get; set; }
        public string? ServiceName { get; set; }
    }
}