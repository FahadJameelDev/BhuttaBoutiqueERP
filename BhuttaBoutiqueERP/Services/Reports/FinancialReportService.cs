using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.ViewModels.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class FinancialReportService : IFinancialReportService
    {
        private readonly AppDbContext _context;

        public FinancialReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FinancialReportVM> GetFinancialSummaryReportAsync()
        {
            // 1. Calculate Real Revenue from Payments (or Total Order amounts)
            decimal totalRevenue = await _context.Payments
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Fallback: If payments table is empty, calculate total revenue from completed/confirmed orders
            if (totalRevenue == 0)
            {
                totalRevenue = await _context.Orders
                    .SumAsync(o => (decimal?)o.Total) ?? 0;
            }

            // 2. Calculate Real Expenses from Payroll & Stock Purchases
            decimal totalPayrollExpenses = await _context.Payrolls
                .SumAsync(p => (decimal?)p.NetSalary) ?? 0;

            decimal totalStockExpenses = await _context.StockIns
                .SumAsync(s => (decimal?)(s.Quantity * s.Price)) ?? 0;

            decimal totalExpenses = totalPayrollExpenses + totalStockExpenses;

            // 3. Fetch Real Recent Payments as Financial Ledger Transactions
            var transactionsList = await _context.Payments
                .OrderByDescending(p => p.Date)
                .Take(20)
                .Select(p => new FinancialTransactionRowVM
                {
                    Id = p.Id,
                    TransactionRef = $"TXN-PAY-{p.Id:D5}",
                    Category = p.Method ?? "Order Payment",
                    Amount = p.Amount,
                    Date = p.Date,
                    Status = string.IsNullOrEmpty(p.Status) ? "Completed ✅" : p.Status
                })
                .ToListAsync();

            // Net Profit Calculation
            decimal netProfit = totalRevenue - totalExpenses;

            return new FinancialReportVM
            {
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                NetProfit = netProfit,
                Transactions = transactionsList
            };
        }
    }
}