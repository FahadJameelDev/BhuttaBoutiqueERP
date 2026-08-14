using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Inventory.ViewModels
{
    public class StockReturnViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Item is required")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Stock Out reference is required")]
        public int StockOutId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;

        // For dropdowns & display
        public string? ItemName { get; set; }
        public string? StockOutReference { get; set; }
    }
}