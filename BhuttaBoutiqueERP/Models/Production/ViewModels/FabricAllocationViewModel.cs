using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Production.ViewModels
{
    public class FabricAllocationViewModel
    {
        [Required(ErrorMessage = "Demand ID is required")]
        public int DemandId { get; set; }

        [Required(ErrorMessage = "Allocated quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Allocated quantity must be greater than 0")]
        public decimal AllocatedQuantity { get; set; }

        public string? Remarks { get; set; }

        // For display
        public string? ItemName { get; set; }
        public string? OrderNumber { get; set; }
        public decimal DemandQuantity { get; set; }
    }
}