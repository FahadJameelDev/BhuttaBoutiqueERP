using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Services.HR
{
    public interface IAdvanceService
    {
        Task<IEnumerable<Advance>> GetAllAsync();
        Task<Advance?> GetByIdAsync(int id);
        Task AddAsync(Advance advance);
        Task UpdateAsync(Advance advance);
        Task DeleteAsync(int id);
    }
}