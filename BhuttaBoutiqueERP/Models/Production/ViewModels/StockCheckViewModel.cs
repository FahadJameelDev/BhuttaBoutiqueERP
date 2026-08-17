namespace BhuttaBoutiqueERP.Models.Production.ViewModels
{
    public class StockCheckViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Stock { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal AvailableStock { get; set; }
    }
}