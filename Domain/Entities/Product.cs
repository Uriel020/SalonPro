using System;

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
    }
}
