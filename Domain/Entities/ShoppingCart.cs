using System;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; } // Identificador �nico del carrito
        public int UserId { get; set; } // Relaci�n con el usuario
        public int StatusId { get; set; } // Relaci�n con el estado del carrito

        public User User { get; set; } // Propiedad de navegaci�n para el usuario
        public CartStatus CartStatus { get; set; } // Propiedad de navegaci�n para el estado del carrito
    }
}
