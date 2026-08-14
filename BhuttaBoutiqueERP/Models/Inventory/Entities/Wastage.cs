using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Inventory.Entities
{
    public class Wastage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }
    }
}