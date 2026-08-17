using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhuttaBoutiqueERP.Models.HR.ViewModels
{
    public class LeaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee selection is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required, MaxLength(30)]
        [Display(Name = "Leave Type")]
        public string Type { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [Required, DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public IEnumerable<SelectListItem>? EmployeeList { get; set; }
    }
}