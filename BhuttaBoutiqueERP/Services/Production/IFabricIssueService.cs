using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Production.Entities;
using BhuttaBoutiqueERP.Models.Production.ViewModels;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IFabricIssueService
    {
        Task<IEnumerable<FabricIssue>> GetAllAsync();
        Task<FabricIssue?> GetByIdAsync(int id);
        Task<FabricIssue> CreateAsync(FabricIssueViewModel model);
        Task<FabricIssue> UpdateAsync(FabricIssueViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<FabricIssue>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<FabricIssue>> GetByEmployeeIdAsync(int employeeId);
    }
}