namespace gozba_na_klik_backend.DTOs
{
    public class RestaurantUpdateDTO
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int OwnerId { get; set; }
    }
}