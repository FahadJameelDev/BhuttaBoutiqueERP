using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.HR.Entities
{
    public class Advance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        public string Type { get; set; } = "Advance"; // Advance, Loan

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Settled

        [Required, MaxLength(20)]
        public string DeductedInMonth { get; set; } = string.Empty; // e.g. "September-2026"
    }
}