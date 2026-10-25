using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public eOrderStatus Status { get; set; } = eOrderStatus.Pending;

        public decimal TotalAmount { get; set; }

        public Store Store { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
