using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public interface IDesignLibraryService
    {
        Task<IEnumerable<DesignLibrary>> GetAllAsync();
        Task<DesignLibrary?> GetByIdAsync(int id);
        Task<DesignLibrary> CreateAsync(DesignLibraryViewModel model);
        Task<DesignLibrary> UpdateAsync(DesignLibraryViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}