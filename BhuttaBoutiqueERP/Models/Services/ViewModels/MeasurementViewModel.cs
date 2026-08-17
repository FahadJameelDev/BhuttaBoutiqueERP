using System;
using System.ComponentModel.DataAnnotations;

namespace BhuttaBoutiqueERP.Models.Services.ViewModels
{
    public class MeasurementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }

        // Male Measurements
        public decimal? Chest { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hips { get; set; }
        public decimal? Shoulder { get; set; }
        public decimal? Armhole { get; set; }
        public decimal? Neck { get; set; }
        public decimal? KurtaLength { get; set; }
        public decimal? ShalwarLength { get; set; }
        public decimal? Paicha { get; set; }
        public decimal? SleeveLength { get; set; }

        // Female Measurements
        public decimal? Bust { get; set; }
        public decimal? KameezLength { get; set; }
        public decimal? DupattaLength { get; set; }
        public decimal? Damman { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        // For dropdowns & display
        public string? CustomerName { get; set; }
        public string? OrderNumber { get; set; }
    }
}