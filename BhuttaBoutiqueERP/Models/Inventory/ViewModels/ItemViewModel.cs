using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Inventory.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Stock must be a positive value")]
        public decimal Stock { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [MaxLength(20, ErrorMessage = "Unit cannot exceed 20 characters")]
        public string Unit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reorder level is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Reorder level must be a positive value")]
        public decimal ReorderLevel { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;
    }
}