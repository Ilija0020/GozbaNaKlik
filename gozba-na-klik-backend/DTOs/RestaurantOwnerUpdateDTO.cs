namespace gozba_na_klik_backend.DTOs
{
    public class RestaurantOwnerUpdateDTO
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public string? Description { get; set; }
    }
}