using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhuttaBoutiqueERP.Models.HR.ViewModels
{
    public class CommissionRateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Service selection is required.")]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Display(Name = "Service Name")]
        public string? ServiceName { get; set; } // For displaying service name in tables/lists

        [Required, MaxLength(20)]
        public string Complexity { get; set; } = string.Empty;

        [Required]
        [Range(0, 100000)]
        public decimal Rate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";

        public IEnumerable<SelectListItem>? ServiceList { get; set; }
    }
}