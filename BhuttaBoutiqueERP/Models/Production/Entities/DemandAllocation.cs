using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Services.Entities;

namespace BhuttaBoutiqueERP.Models.Production.Entities
{
    public class DemandAllocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public decimal DemandQuantity { get; set; }

        [Required]
        public decimal AllocatedQuantity { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending / Allocated / Completed

        [Required]
        public DateTime Date { get; set; }

        // ── Navigation Properties ──
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }
    }
}