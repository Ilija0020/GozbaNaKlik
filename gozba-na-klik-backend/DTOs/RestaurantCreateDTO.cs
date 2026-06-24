namespace gozba_na_klik_backend.DTOs
{
    public class RestaurantCreateDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Address { get; set; }
        public int OwnerId { get; set; }
    }
}