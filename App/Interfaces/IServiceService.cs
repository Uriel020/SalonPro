using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IServiceService
    {
        Task<ServiceDto> CreateServiceAsync(ServiceDto serviceDto);
        Task<ServiceDto> GetServiceAsync(int serviceId);
        Task<List<ServiceDto>> GetAllServicesAsync();
        Task<bool> UpdateServiceAsync(int serviceId, ServiceDto serviceDto);
        Task<bool> DeleteServiceAsync(int serviceId);
    }
}