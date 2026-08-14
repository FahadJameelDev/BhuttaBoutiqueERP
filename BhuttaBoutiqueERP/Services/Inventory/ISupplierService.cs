using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> CreateAsync(SupplierViewModel model);
        Task<Supplier> UpdateAsync(SupplierViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}