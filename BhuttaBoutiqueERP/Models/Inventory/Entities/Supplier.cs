using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Inventory.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Contact { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<StockIn>? StockIns { get; set; }
    }
}