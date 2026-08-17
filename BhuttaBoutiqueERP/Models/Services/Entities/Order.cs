using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public decimal Advance { get; set; }

        [Required]
        public decimal Remaining { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("ServiceId")]
        public virtual ServiceCatalog? Service { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<Measurement>? Measurements { get; set; }
    }
}