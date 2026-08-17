using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class BulkOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TotalItems { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public decimal? Discount { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}