using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhuttaBoutiqueERP.Models.HR.ViewModels
{
    public class AdvanceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee selection is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "Request Type")]
        public string Type { get; set; } = "Advance";

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [MaxLength(20)]
        [Display(Name = "Deducted In Month")]
        public string? DeductedInMonth { get; set; } // Nullable because it's assigned later

        public IEnumerable<SelectListItem>? EmployeeList { get; set; }
    }
}