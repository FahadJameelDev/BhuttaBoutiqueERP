using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = "Regular"; // Regular / Bulk

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation Properties
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<BulkOrder>? BulkOrders { get; set; }
        public virtual ICollection<Measurement>? Measurements { get; set; }
    }
}