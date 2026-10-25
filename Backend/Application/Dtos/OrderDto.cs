using Domain.Enums;

namespace Application.Dtos
{
    public class OrderDto
    {
        public Guid? Id { get; set; }
        public Guid StoreId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public eOrderStatus Status { get; set; } = eOrderStatus.Pending;
        public decimal TotalAmount { get; set; }
    }
}
