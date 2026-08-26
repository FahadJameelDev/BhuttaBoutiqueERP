namespace BhuttaBoutiqueERP.Models.Reports.ViewModels
{
    public class DashboardOverviewViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int LowStockItemsCount { get; set; }
        public int ActiveEmployees { get; set; }
        public int PresentEmployeesToday { get; set; }

        public List<RecentOrderDto> RecentOrders { get; set; } = new();
        public List<LowStockItemDto> LowStockItems { get; set; } = new();
    }

    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class LowStockItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
    }
}