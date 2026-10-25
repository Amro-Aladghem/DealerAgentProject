namespace Application.Dtos
{
    public class ComplaintDto
    {
        public Guid? Id { get; set; }
        public Guid StoreId { get; set; }
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
