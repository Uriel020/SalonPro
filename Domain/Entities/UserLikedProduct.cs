using System;

namespace Domain.Entities
{
    public class UserLikedProduct
    {
        public int UserId { get; set; } // Relaci�n con el usuario
        public int ProductId { get; set; } // Relaci�n con el producto

        public User User { get; set; } // Propiedad de navegaci�n para el usuario
        public Product Product { get; set; } // Propiedad de navegaci�n para el producto
    }
}
