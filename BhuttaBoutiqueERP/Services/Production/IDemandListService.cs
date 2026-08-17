using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.Entities;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IDemandListService
    {
        Task<IEnumerable<DemandAllocation>> GetAllAsync();
        Task<DemandAllocation?> GetByIdAsync(int id);
        Task<IEnumerable<DemandAllocation>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<DemandAllocation>> GetPendingDemandsAsync();
    }
}