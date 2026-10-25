using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Domain.Entities
{
    public class Store
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ProductsUrl { get; set; }

        public string? ComplaintsFileUrl { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();

        public ICollection<CarAccessory> CarAccessories { get; set; } = new List<CarAccessory>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
