using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class CartStatus
    {
        public int Id { get; set; } // Identificador único
        public string Name { get; set; } // Nombre del estado del carrito (Ej. "Activo", "Procesado")
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el estado
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Fecha de última actualización
        public string UpdatedBy { get; set; } = "System"; // Usuario que realizó la última actualización
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación lógica
        public string DeletedBy { get; set; } // Usuario que eliminó el estado

        // Relación con ShoppingCart (opcional, si se necesita una relación bidireccional)
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
