using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public interface IServiceCatalogService
    {
        Task<IEnumerable<ServiceCatalog>> GetAllAsync();
        Task<ServiceCatalog?> GetByIdAsync(int id);
        Task<ServiceCatalog> CreateAsync(ServiceCatalogViewModel model);
        Task<ServiceCatalog> UpdateAsync(ServiceCatalogViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}