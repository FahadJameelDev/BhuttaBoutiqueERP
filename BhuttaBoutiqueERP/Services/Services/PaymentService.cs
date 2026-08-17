using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> CreateAsync(PaymentViewModel model)
        {
            var payment = new Payment
            {
                OrderId = model.OrderId,
                Amount = model.Amount,
                Method = model.Method,
                Date = model.Date,
                Status = model.Status
            };

            _context.Payments.Add(payment);

            // ✅ Order Remaining Update (Business Logic)
            var order = await _context.Orders.FindAsync(model.OrderId);
            if (order != null)
            {
                order.Remaining -= model.Amount;
                if (order.Remaining <= 0)
                {
                    order.Remaining = 0;
                    order.Status = "Completed";
                }
            }

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdateAsync(PaymentViewModel model)
        {
            var payment = await _context.Payments.FindAsync(model.Id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {model.Id} not found.");

            payment.Amount = model.Amount;
            payment.Method = model.Method;
            payment.Status = model.Status;

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return false;

            // ✅ Payment delete hone par order remaining restore karein
            var order = await _context.Orders.FindAsync(payment.OrderId);
            if (order != null)
            {
                order.Remaining += payment.Amount;
                if (order.Remaining > 0 && order.Status == "Completed")
                {
                    order.Status = "Pending";
                }
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }
    }
}