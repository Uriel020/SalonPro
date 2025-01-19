using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto);
        Task<AppointmentDto> GetAppointmentAsync(int appointmentId);
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();
        Task<bool> UpdateAppointmentAsync(int appointmentId, AppointmentDto appointmentDto);
        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}
