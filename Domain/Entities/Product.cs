using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } // Identificador único
        public string Name { get; set; } // Nombre del producto
        public string Description { get; set; } // Descripción del producto
        public int CategoryId { get; set; } // Relación con la categoría
        public Category Category { get; set; } // Propiedad de navegación para la relación con la categoría
        public string SKU { get; set; } // Código único del producto
        public decimal Price { get; set; } // Precio del producto
        public int Stock { get; set; } // Cantidad en stock
        public string Color { get; set; } // Color del producto
        public string Brand { get; set; } // Marca del producto
        public decimal Weight { get; set; } // Peso del producto
        public string Size { get; set; } // Tamaño del producto
        public string ImageUrl { get; set; } // URL de la imagen del producto
        public DateTime ExpiryDate { get; set; } // Fecha de caducidad del producto
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el registro
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Fecha de última actualización
        public string UpdatedBy { get; set; } = "System"; // Usuario que realizó la última actualización
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación lógica
        public string DeletedBy { get; set; } // Usuario que eliminó lógicamente el registro

        // Relación con CartDetails
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
