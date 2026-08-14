using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Inventory.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact is required")]
        [MaxLength(50, ErrorMessage = "Contact cannot exceed 50 characters")]
        [Phone(ErrorMessage = "Invalid contact number format")]
        public string Contact { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;
    }
}