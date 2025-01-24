using System;
using Domain.Entities;

namespace Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; } // Identificador único
        public string Title { get; set; } // Título de la notificación
        public string Message { get; set; } // Mensaje de la notificación
        public NotificationType Type { get; set; } // Tipo de notificación (enum)
        public bool IsRead { get; set; } // Estado de lectura
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación
    }

    public enum NotificationType
    {
        Alert,
        Reminder,
        Warning,
        Info
    }
}
