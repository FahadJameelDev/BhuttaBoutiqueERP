namespace BhuttaBoutiqueERP.ViewModels.Reports
{
    public class ProductionReportVM
    {
        public int TotalOrdersInProduction { get; set; }
        public int CompletedOrdersCount { get; set; }
        public int TotalFabricIssuedMeters { get; set; }
        public List<ProductionItemRowVM> Items { get; set; } = new();
    }

    public class ProductionItemRowVM
    {
        public int Id { get; set; }
        public required string OrderNumber { get; set; }
        public required string DesignName { get; set; }
        public required string AssignedTailor { get; set; }
        public required string CurrentStage { get; set; }
        public required string Status { get; set; }
    }
}