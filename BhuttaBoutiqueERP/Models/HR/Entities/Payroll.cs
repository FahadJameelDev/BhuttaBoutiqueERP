using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.HR.Entities
{
    public class Payroll
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Overtime { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deductions { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; }

        [Required, MaxLength(20)]
        public string Month { get; set; } = string.Empty; // e.g. "August-2026"

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Unpaid"; // Unpaid, Paid
    }
}