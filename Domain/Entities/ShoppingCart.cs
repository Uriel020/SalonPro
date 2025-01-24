using System;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; } // Identificador único del carrito
        public int UserId { get; set; } // Relación con el usuario
        public int StatusId { get; set; } // Relación con el estado del carrito

        public User User { get; set; } // Propiedad de navegación para el usuario
        public CartStatus CartStatus { get; set; } // Propiedad de navegación para el estado del carrito
    }
}
