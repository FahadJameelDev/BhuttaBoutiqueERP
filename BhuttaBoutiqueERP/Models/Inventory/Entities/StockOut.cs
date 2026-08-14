using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Inventory.Entities
{
    public class StockOut
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        public int? OrderId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public int? IssuedTo { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        public virtual ICollection<StockReturn>? StockReturns { get; set; }
    }
}