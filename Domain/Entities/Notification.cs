using System;

namespace Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; } // Identificador único
        public int UserId { get; set; } // Relación con el usuario
        public string Title { get; set; } // Título de la notificación
        public string Message { get; set; } // Mensaje de la notificación
        public NotificationType Type { get; set; } // Tipo de notificación (enum)
        public bool IsRead { get; set; } = false; // Estado de lectura
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación

        // Relación con el usuario
        public User User { get; set; }
    }

    public enum NotificationType
    {
        Alert,
        Reminder,
        Warning,
        Info
    }
}
