using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BhuttaBoutiqueERP.Models.Services.Entities;

namespace BhuttaBoutiqueERP.Models.Production.Entities
{
    public class ProductionLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Stage { get; set; } = string.Empty; // Cutting / Stitching / Embroidery / Finishing / QC

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty; // Started / Completed / Pending

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // ── Navigation Properties ──
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}