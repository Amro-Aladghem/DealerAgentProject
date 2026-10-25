namespace Application.Dtos
{
    public class StoreDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ProductsUrl { get; set; }
        public string? ComplaintsFileUrl { get; set; }
    }
}
