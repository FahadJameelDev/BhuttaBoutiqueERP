using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhuttaBoutiqueERP.Models.HR.ViewModels
{
    public class AttendanceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee selection is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Display(Name = "Check In")]
        public TimeSpan? CheckIn { get; set; }

        [Display(Name = "Check Out")]
        public TimeSpan? CheckOut { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Present";

        [Range(0, 1440, ErrorMessage = "Late minutes cannot be negative")]
        [Display(Name = "Late Minutes")]
        public int LateMinutes { get; set; } = 0;

        [Range(0, 24, ErrorMessage = "Overtime hours cannot be negative")]
        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; } = 0;

        public IEnumerable<SelectListItem>? EmployeeList { get; set; }
    }
}