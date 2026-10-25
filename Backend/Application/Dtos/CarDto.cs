namespace Application.Dtos
{
    public class CarDto
    {
        public Guid? Id { get; set; }
        public Guid StoreId { get; set; }
        public int CarMakeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? ModelYear { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
    }
}
