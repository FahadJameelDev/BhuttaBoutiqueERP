using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class Measurement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int OrderId { get; set; }

        // Male Measurements
        public decimal? Chest { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hips { get; set; }
        public decimal? Shoulder { get; set; }
        public decimal? Armhole { get; set; }
        public decimal? Neck { get; set; }
        public decimal? KurtaLength { get; set; }
        public decimal? ShalwarLength { get; set; }
        public decimal? Paicha { get; set; }
        public decimal? SleeveLength { get; set; }

        // Female Measurements
        public decimal? Bust { get; set; }
        public decimal? KameezLength { get; set; }
        public decimal? DupattaLength { get; set; }
        public decimal? Damman { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}