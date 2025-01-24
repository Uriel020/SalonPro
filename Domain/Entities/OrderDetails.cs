using System;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; } // Identificador único
        public int OrderId { get; set; } // Relación con la tabla Orders
        public int ProductId { get; set; } // Relación con la tabla Products
        public int Count { get; set; } // Cantidad de productos
        public decimal UnitPrice { get; set; } // Precio unitario
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación
        public string CreatedBy { get; set; } = "System"; // Usuario que creó el registro
        public DateTime? UpdatedAt { get; set; } // Fecha de última actualización
        public string UpdatedBy { get; set; } = "System"; // Usuario que realizó la última actualización
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación lógica
        public string DeletedBy { get; set; } // Usuario que eliminó lógicamente el registro

        // Relación con la tabla Orders
        public Order Order { get; set; }
        // Relación con la tabla Products
        public Product Product { get; set; }
    }
}
