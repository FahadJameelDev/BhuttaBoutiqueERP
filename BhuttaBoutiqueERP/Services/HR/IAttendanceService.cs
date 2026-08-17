using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.HR.Entities;

namespace BhuttaBoutiqueERP.Services.HR
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task DeleteAsync(int id);
    }
}