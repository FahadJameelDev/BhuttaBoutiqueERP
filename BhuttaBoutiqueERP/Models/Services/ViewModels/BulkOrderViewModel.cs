using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.ViewModels
{
    public class BulkOrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Total items is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total items must be at least 1")]
        public int TotalItems { get; set; }

        [Required(ErrorMessage = "Total price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public decimal TotalPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive value")]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Delivery date is required")]
        public DateTime DeliveryDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Pending";

        // For dropdowns & display
        public string? CustomerName { get; set; }
    }
}