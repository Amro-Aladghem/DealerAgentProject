using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CarAccessory
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public string? ProductUrl { get; set; }

        public Store Store { get; set; } = null!;
    }
}
