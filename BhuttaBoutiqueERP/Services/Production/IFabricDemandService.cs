using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IFabricDemandService
    {
        Task<IEnumerable<DemandAllocation>> GetAllAsync();
        Task<DemandAllocation?> GetByIdAsync(int id);
        Task<DemandAllocation> CreateAsync(DemandAllocationViewModel model);
        Task<DemandAllocation> UpdateAsync(DemandAllocationViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<DemandAllocation>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<DemandAllocation>> GetByItemIdAsync(int itemId);
    }
}