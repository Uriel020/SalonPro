using System;

namespace Domain.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; } // Identificador único
        public string Name { get; set; } // Nombre del método de pago
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el registro
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Fecha de última actualización
        public string UpdatedBy { get; set; } = "System"; // Usuario que realizó la última actualización
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación lógica
        public string DeletedBy { get; set; } // Usuario que eliminó lógicamente el registro
    }
}
