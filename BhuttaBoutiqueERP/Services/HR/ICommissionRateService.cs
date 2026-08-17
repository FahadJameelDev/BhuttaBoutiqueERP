using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Services.HR
{
    public interface ICommissionRateService
    {
        Task<IEnumerable<CommissionRate>> GetAllAsync();
        Task<CommissionRate?> GetByIdAsync(int id);
        Task AddAsync(CommissionRate commissionRate);
        Task UpdateAsync(CommissionRate commissionRate);
        Task DeleteAsync(int id);
    }
}