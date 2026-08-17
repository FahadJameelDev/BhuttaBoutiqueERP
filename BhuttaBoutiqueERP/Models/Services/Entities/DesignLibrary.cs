using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.Entities
{
    public class DesignLibrary
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
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty; // Embroidery / Print / Lace Work

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(20)]
        public string Complexity { get; set; } = "Medium"; // Easy / Medium / Hard

        [MaxLength(500)]
        public string? Image { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";
    }
}