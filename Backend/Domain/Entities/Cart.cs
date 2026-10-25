using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public Store Store { get; set; } = null!;

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
