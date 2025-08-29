using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IStaffService
    {
        Task<StaffDto> CreateStaffAsync(StaffDto staffDto);
        Task<StaffDto> GetStaffAsync(int staffId);
        Task<List<StaffDto>> GetAllStaffAsync();
        Task<bool> UpdateStaffAsync(int staffId, StaffDto staffDto);
        Task<bool> DeleteStaffAsync(int staffId);
    }
}
