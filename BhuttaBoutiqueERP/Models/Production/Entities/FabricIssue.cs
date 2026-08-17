using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Services.Entities;

namespace BhuttaBoutiqueERP.Models.Production.Entities
{
    public class FabricIssue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public int? IssuedTo { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending / Completed

        // ── Navigation Properties ──
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [ForeignKey("IssuedTo")]
        public virtual User? Employee { get; set; }
    }
}