using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string ServiceName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Stitching, Ready, Completed

        [Required]
        public decimal Total { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime DeliveryDate { get; set; }
    }
}