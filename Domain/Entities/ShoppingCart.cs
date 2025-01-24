using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; } // Identificador único
        public int UserId { get; set; } // Relación con la tabla Users
        public int CartStatusId { get; set; } // Relación con la tabla CartStatus
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el carrito
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Fecha de última actualización
        public string UpdatedBy { get; set; } = "System"; // Usuario que realizó la última actualización
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación lógica
        public string DeletedBy { get; set; } // Usuario que eliminó lógicamente el carrito

        // Relaciones con otras entidades
        public User User { get; set; } // Relación con User
        public CartStatus CartStatus { get; set; } // Relación con CartStatus
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
