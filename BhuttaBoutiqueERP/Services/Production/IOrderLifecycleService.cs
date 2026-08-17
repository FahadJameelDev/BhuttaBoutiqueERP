using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IOrderLifecycleService
    {
        Task<IEnumerable<OrderLifecycle>> GetAllAsync();
        Task<OrderLifecycle?> GetByIdAsync(int id);
        Task<OrderLifecycle> CreateAsync(OrderLifecycleViewModel model);
        Task<OrderLifecycle> UpdateAsync(OrderLifecycleViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<OrderLifecycle>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderLifecycle>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<OrderLifecycle>> GetByStageAsync(string stage);
    }
}