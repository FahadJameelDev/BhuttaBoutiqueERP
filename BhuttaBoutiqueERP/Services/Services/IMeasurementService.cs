using System.Collections.Generic;
using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.Entities;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public interface IMeasurementService
    {
        Task<IEnumerable<Measurement>> GetAllAsync();
        Task<Measurement?> GetByIdAsync(int id);
        Task<Measurement> CreateAsync(MeasurementViewModel model);
        Task<Measurement> UpdateAsync(MeasurementViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Measurement>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Measurement>> GetByOrderIdAsync(int orderId);
    }
}