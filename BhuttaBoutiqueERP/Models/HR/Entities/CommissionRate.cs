using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BhuttaBoutiqueERP.Models.Services.Entities;

namespace BhuttaBoutiqueERP.Models.HR.Entities
{
    public class CommissionRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; } // Links to ServiceCatalog

        [ForeignKey("ServiceId")]
        public virtual ServiceCatalog? ServiceCatalog { get; set; }

        [Required, MaxLength(20)]
        public string Complexity { get; set; } = string.Empty; // Easy, Medium, Hard

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive
    }
}