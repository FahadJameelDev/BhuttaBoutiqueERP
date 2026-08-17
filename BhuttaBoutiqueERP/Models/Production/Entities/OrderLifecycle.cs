using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Models.Services.Entities;

namespace BhuttaBoutiqueERP.Models.Production.Entities
{
    public class OrderLifecycle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Stage { get; set; } = "Cutting"; // Cutting / Stitching / Embroidery / Finishing / QC

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending / InProgress / Completed

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // ── Navigation Properties ──
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual User? Employee { get; set; }
    }
}