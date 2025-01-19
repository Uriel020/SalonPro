using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface CustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);
        Task<CustomerDto> GetCustomerAsync(int customerId);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<bool> UpdateCustomerAsync(int customerId, CustomerDto customerDto);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}
