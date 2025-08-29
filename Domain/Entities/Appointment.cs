using System;
using Domain.Entities;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ServiceId { get; set; } // Relación con Service
        public int UserId { get; set; } // Relación con User (Staff)
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; } // Estado de la cita
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; } // Comentarios sobre la cita
        public int DurationInMinutes { get; set; } // Duración en minutos
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación

        // Propiedades de navegación
        public User User { get; set; } // Relación con User (Staff)
        public Service Service { get; set; } // Relación con Service
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
