using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Services.HR
{
    public interface ILeaveService
    {
        Task<IEnumerable<Leave>> GetAllAsync();
        Task<Leave?> GetByIdAsync(int id);
        Task AddAsync(Leave leave);
        Task UpdateAsync(Leave leave);
        Task DeleteAsync(int id);
    }
}