using System.Threading.Tasks;
using BhuttaBoutiqueERP.Models.Services.ViewModels;

namespace BhuttaBoutiqueERP.Services.Services
{
    public interface IServiceReportService
    {
        Task<ServiceReportViewModel> GetReportDataAsync();
    }
}