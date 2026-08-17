using System.Threading.Tasks;

namespace BhuttaBoutiqueERP.Services.Production
{
    public interface IStockCheckService
    {
        Task<bool> CheckAvailabilityAsync(int itemId, decimal quantity);
        Task<decimal> GetAvailableStockAsync(int itemId);
        Task<bool> ValidateDemandAsync(int orderId);
    }
}