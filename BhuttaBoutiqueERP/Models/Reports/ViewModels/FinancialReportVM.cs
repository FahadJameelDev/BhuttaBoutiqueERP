namespace BhuttaBoutiqueERP.ViewModels.Reports
{
    public class FinancialTransactionRowVM
    {
        public int Id { get; set; }
        public string TransactionRef { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class FinancialReportVM
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public List<FinancialTransactionRowVM> Transactions { get; set; } = new();
    }
}