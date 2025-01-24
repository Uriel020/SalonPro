using System;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int StaffId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; } // Cambiado a enum
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; } // Podrías añadir una longitud máxima
        public int DurationInMinutes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Valor predeterminado
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
