using System;
using System.Collections.Generic;

namespace BhuttaBoutiqueERP.Models.Production.ViewModels
{
    public class ProductionReportViewModel
    {
        // ── Widgets ──
        public int TotalOrdersInProduction { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }

        // ── Stage-wise counts ──
        public int CuttingCount { get; set; }
        public int StitchingCount { get; set; }
        public int EmbroideryCount { get; set; }
        public int FinishingCount { get; set; }
        public int QCCount { get; set; }

        // ── Recent Activities ──
        public List<ProductionActivity> RecentActivities { get; set; } = new();

        // ── Report Period ──
        public string? ReportPeriod { get; set; }
    }

    public class ProductionActivity
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}