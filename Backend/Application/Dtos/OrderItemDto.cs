using Domain.Enums;

namespace Application.Dtos
{
    public class OrderItemDto
    {
        public Guid? Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public eOrderItemType ItemType { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
