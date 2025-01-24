using System;

namespace Domain.Entities
{
    public class UserLikedProduct
    {
        public int UserId { get; set; } // Relación con el usuario
        public int ProductId { get; set; } // Relación con el producto

        public User User { get; set; } // Propiedad de navegación para el usuario
        public Product Product { get; set; } // Propiedad de navegación para el producto
    }
}
