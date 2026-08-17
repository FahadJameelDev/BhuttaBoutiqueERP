using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.HR.Entities
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Present"; // Present, Absent, HalfDay, Leave

        public int LateMinutes { get; set; } = 0;
        public decimal OvertimeHours { get; set; } = 0;
    }
}