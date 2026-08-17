using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class ServiceCatalog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Time { get; set; } // in days

        [Required]
        [MaxLength(20)]
        public string Complexity { get; set; } = "Medium"; // Easy / Medium / Hard

        public decimal FabricRequire { get; set; } // in meters

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation Properties
        public virtual ICollection<Order>? Orders { get; set; }
    }
}