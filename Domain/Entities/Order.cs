using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public int StateId { get; set; }
        public int PaymentMethodId { get; set; }
        public int CartId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = "System";
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

        public User User { get; set; }
        public OrderStatus State { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ShoppingCart Cart { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
