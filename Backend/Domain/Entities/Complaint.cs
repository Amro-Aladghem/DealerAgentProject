using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Complaint
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? CustomerName { get; set; }

        public string? CustomerEmail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Store Store { get; set; } = null!;
    }
}
