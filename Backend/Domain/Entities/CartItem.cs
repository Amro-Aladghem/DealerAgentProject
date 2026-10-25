using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public eOrderItemType ItemType { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; } = null!;
    }
}
