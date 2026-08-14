using System;
using System.Collections.Generic;

namespace BhuttaBoutiqueERP.Models.ViewModels
{
    public class DashboardViewModel
    {
        // ── Widgets ──
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalEmployees { get; set; }

        // ── Chart Data (Initialized to avoid null reference) ──
        public List<WeeklySale> WeeklySales { get; set; } = new();

        // ── Recent Orders (Initialized to avoid null reference) ──
        public List<RecentOrder> RecentOrders { get; set; } = new();
    }

    // ── Inner Class: Weekly Sale ──
    public class WeeklySale
    {
        public string Day { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    // ── Inner Class: Recent Order ──
    public class RecentOrder
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}