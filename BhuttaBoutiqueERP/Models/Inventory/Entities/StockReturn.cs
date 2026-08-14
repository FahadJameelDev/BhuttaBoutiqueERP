using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Inventory.Entities
{
    public class StockReturn
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int StockOutId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [ForeignKey("StockOutId")]
        public virtual StockOut? StockOut { get; set; }
    }
}