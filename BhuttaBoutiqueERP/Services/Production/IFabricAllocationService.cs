using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IFabricAllocationService
    {
        Task<IEnumerable<DemandAllocation>> GetAllAsync();
        Task<DemandAllocation?> GetByIdAsync(int id);
        Task<DemandAllocation> AllocateAsync(FabricAllocationViewModel model);
        Task<DemandAllocation> UpdateAllocationAsync(DemandAllocationViewModel model);
        Task<bool> DeallocateAsync(int id);
        Task<IEnumerable<DemandAllocation>> GetByOrderIdAsync(int orderId);
    }
}