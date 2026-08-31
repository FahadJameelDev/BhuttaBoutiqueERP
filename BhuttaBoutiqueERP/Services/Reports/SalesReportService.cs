using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Reports;

namespace BhuttaBoutiqueERP.Services.Reports
{
    public class SalesReportService : ISalesReportService
    {
        private readonly AppDbContext _context;

        public SalesReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SalesReportViewModel> GetSalesReportAsync(DateTime? startDate, DateTime? endDate, int? customerId, string? paymentStatus)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .AsQueryable();

            // 1. Date Range Filter
            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var endDateTime = endDate.Value.Date.AddDays(1).AddSeconds(-1);
                query = query.Where(o => o.OrderDate <= endDateTime);
            }

            // 2. Customer Filter
            if (customerId.HasValue && customerId.Value > 0)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            // 3. Payment Status / Status Filter
            if (!string.IsNullOrEmpty(paymentStatus) && paymentStatus != "All Statuses" && paymentStatus != "All")
            {
                query = query.Where(o => o.Status == paymentStatus || o.PaymentMethod == paymentStatus);
            }

            var ordersList = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Mapping to DTO using your exact Order model properties
            var reportItems = ordersList.Select(o => new SaleReportItemDto
            {
                OrderId = o.Id,
                OrderNumber = o.OrderNumber ?? $"ORD-{o.Id}",
                CustomerName = (o.Customer != null && o.Customer.Name != null) ? o.Customer.Name : "Walk-in Customer",
                CustomerPhone = (o.Customer != null && o.Customer.Phone != null) ? o.Customer.Phone : "N/A",
                OrderDate = o.OrderDate,
                TotalAmount = o.Total,
                PaidAmount = o.Advance,
                RemainingAmount = o.Remaining,
                OrderStatus = o.Status ?? "Pending",
                PaymentStatus = o.PaymentMethod ?? "Paid"
            }).ToList();

            // Summary Totals Calculation
            var viewModel = new SalesReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                CustomerId = customerId,
                PaymentStatus = paymentStatus,
                TotalOrdersCount = reportItems.Count,
                TotalSalesRevenue = reportItems.Sum(x => x.TotalAmount),
                TotalPaymentsReceived = reportItems.Sum(x => x.PaidAmount),
                TotalPendingDues = reportItems.Sum(x => x.RemainingAmount),
                SalesRecords = reportItems
            };

            return viewModel;
        }
    }
}