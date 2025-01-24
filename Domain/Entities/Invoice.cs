using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; } // Identificador único
        public string InvoiceNumber { get; set; } // Número único de factura
        public int UserId { get; set; } // Relación con el usuario
        public DateTime InvoiceDate { get; set; } // Fecha de emisión
        public DateTime? DueDate { get; set; } // Fecha límite de pago (opcional)
        public decimal TotalAmount { get; set; } // Monto total
        public decimal TaxAmount { get; set; } // Monto de impuestos
        public decimal DiscountAmount { get; set; } // Monto de descuento
        public InvoiceStatus Status { get; set; } // Estado de la factura
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación
        public User User { get; set; }

        // Relación con Order
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    // Relación con User
    
    public enum InvoiceStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}
