using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Inventory.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Total { get; set; }

        // Navigation Properties
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }
    }
}