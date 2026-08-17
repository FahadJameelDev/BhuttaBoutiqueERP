using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment> CreateAsync(PaymentViewModel model);
        Task<Payment> UpdateAsync(PaymentViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);
    }
}