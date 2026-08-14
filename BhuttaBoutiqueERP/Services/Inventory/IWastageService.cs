using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public interface IWastageService
    {
        Task<IEnumerable<Wastage>> GetAllAsync();
        Task<Wastage?> GetByIdAsync(int id);
        Task<Wastage> CreateAsync(WastageViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Wastage>> GetByItemIdAsync(int itemId);
    }
}