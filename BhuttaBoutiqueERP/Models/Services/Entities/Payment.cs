using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Method { get; set; } = string.Empty; // Cash / JazzCash / EasyPaisa / Bank

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Completed";

        // Navigation Properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}