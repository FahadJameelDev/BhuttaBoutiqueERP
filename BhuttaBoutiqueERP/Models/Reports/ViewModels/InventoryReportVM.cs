namespace BhuttaBoutiqueERP.ViewModels.Reports
{
    public class InventoryReportVM
    {
        public int TotalItems { get; set; }
        public int LowStockCount { get; set; }
        public decimal TotalWastageValue { get; set; }
        public List<InventoryItemRowVM> Items { get; set; } = new();
    }

    public class InventoryItemRowVM
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal Stock { get; set; }
        public decimal ReorderLevel { get; set; }
        public required string Status { get; set; }
    }
}