using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public interface IStockInService
    {
        Task<IEnumerable<StockIn>> GetAllAsync();
        Task<StockIn?> GetByIdAsync(int id);
        Task<StockIn> CreateAsync(StockInViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<StockIn>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<StockIn>> GetBySupplierIdAsync(int supplierId);
    }
}