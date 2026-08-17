using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public interface IBulkOrderService
    {
        Task<IEnumerable<BulkOrder>> GetAllAsync();
        Task<BulkOrder?> GetByIdAsync(int id);
        Task<BulkOrder> CreateAsync(BulkOrderViewModel model);
        Task<BulkOrder> UpdateAsync(BulkOrderViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<BulkOrder>> GetByCustomerIdAsync(int customerId);
    }
}