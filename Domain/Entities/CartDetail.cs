using System;

namespace Domain.Entities
{
    public class CartDetail
    {
        public int Id { get; set; } // Identificador único
        public int CartId { get; set; } // Relación con ShoppingCart
        public int ProductId { get; set; } // Relación con Products
        public int Quantity { get; set; } // Cantidad de productos
        public decimal UnitPrice { get; set; } // Precio unitario del producto
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el detalle
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Fecha de última actualización
        public string UpdatedBy { get; set; } = "System"; // Usuario que realizó la última actualización
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación lógica
        public string DeletedBy { get; set; } // Usuario que eliminó lógicamente el detalle

        // Relaciones con otras entidades
        public ShoppingCart ShoppingCart { get; set; } // Relación con ShoppingCart
        public Product Product { get; set; } // Relación con Products
    }
}
