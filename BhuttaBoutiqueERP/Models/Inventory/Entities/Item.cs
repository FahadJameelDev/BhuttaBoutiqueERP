using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Inventory.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public decimal Stock { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty;

        [Required]
        public decimal ReorderLevel { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        // Navigation Properties (Nullable to avoid CS8618)
        public virtual ICollection<StockIn>? StockIns { get; set; }
        public virtual ICollection<StockOut>? StockOuts { get; set; }
        public virtual ICollection<StockReturn>? StockReturns { get; set; }
        public virtual ICollection<Wastage>? Wastages { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}