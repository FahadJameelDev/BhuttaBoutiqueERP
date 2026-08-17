using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.ViewModels
{
    public class DesignLibraryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Design name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [MaxLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Complexity is required")]
        [MaxLength(20, ErrorMessage = "Complexity cannot exceed 20 characters")]
        public string Complexity { get; set; } = "Medium";

        [MaxLength(500, ErrorMessage = "Image path cannot exceed 500 characters")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active";
    }
}