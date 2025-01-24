using System;
using Domain.Entities;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; } // Identificador único
        public DateTime OrderDate { get; set; } // Fecha de la orden
        public decimal Total { get; set; } // Monto total de la orden
        public decimal TaxAmount { get; set; } // Monto de impuestos, si aplica
        public decimal DiscountAmount { get; set; } // Descuento aplicado, si aplica
        public int UserId { get; set; } // Relación con el usuario (si corresponde)
        public User User { get; set; } // Propiedad de navegación para la relación con el usuario
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
