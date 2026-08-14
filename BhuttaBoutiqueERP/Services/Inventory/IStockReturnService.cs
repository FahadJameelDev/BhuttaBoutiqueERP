using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Inventory.Entities;
using BhuttaBoutiqueERP.Models.Inventory.ViewModels;

namespace BhuttaBoutiqueERP.Services.Inventory
{
    public interface IStockReturnService
    {
        Task<IEnumerable<StockReturn>> GetAllAsync();
        Task<StockReturn?> GetByIdAsync(int id);
        Task<StockReturn> CreateAsync(StockReturnViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<StockReturn>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<StockReturn>> GetByStockOutIdAsync(int stockOutId);
    }
}