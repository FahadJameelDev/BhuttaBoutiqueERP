using System;
using System.Collections.Generic;

namespace BhuttaBoutiqueERP.Models.Reports
{
    public class SalesReportViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CustomerId { get; set; }
        public string? PaymentStatus { get; set; }

        public decimal TotalSalesRevenue { get; set; }
        public int TotalOrdersCount { get; set; }
        public decimal TotalPaymentsReceived { get; set; }
        public decimal TotalPendingDues { get; set; }

        public List<SaleReportItemDto> SalesRecords { get; set; } = new List<SaleReportItemDto>();
    }

    public class SaleReportItemDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
    }
}