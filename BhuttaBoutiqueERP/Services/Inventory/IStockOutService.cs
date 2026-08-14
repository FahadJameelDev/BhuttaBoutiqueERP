using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public interface IStockOutService
    {
        Task<IEnumerable<StockOut>> GetAllAsync();
        Task<StockOut?> GetByIdAsync(int id);
        Task<StockOut> CreateAsync(StockOutViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<StockOut>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<StockOut>> GetByOrderIdAsync(int orderId);
    }
}