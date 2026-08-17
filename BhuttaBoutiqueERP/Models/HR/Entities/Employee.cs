using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // 📌 Yeh namespace zaroori hai [NotMapped] ke liye

namespace BhuttaBoutiqueERP.Models.HR.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // 📌 Yeh property add kar di hai taake Attendance aur Payroll mein 'FullName' ka error khatam ho jaye
        [NotMapped]
        public string FullName => Name;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string CNIC { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = string.Empty; // e.g. Karigar, Tailor, Manager

        [MaxLength(200)]
        public string Skills { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string EmploymentType { get; set; } = string.Empty; // Fixed, Commission Only, Mixed

        [Range(0, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; } = DateTime.Now;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive

        // Navigation Properties
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
        public ICollection<Advance> Advances { get; set; } = new List<Advance>();
    }
}