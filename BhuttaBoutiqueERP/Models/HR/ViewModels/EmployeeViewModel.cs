using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.HR.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        [Display(Name = "CNIC Number")]
        public string CNIC { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Skills { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; } = string.Empty;

        [Range(0, 1000000)]
        [Display(Name = "Base Salary")]
        public decimal BaseSalary { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; } = DateTime.Now;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active";
    }
}