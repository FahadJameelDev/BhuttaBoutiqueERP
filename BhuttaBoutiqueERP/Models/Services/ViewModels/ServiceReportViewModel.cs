using System;
using System.Collections.Generic;

namespace BhuttaBoutiqueERP.Models.Services.ViewModels
{
    public class ServiceReportViewModel
    {
        // ── Summary Widgets ──
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }

        // ── Top Services ──
        public List<ServiceStat> TopServices { get; set; } = new();

        // ── Recent Orders ──
        public List<OrderSummary> RecentOrders { get; set; } = new();
    }

    public class ServiceStat
    {
        public string ServiceName { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class OrderSummary
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}