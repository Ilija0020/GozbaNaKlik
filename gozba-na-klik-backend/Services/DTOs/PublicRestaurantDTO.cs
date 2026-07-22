namespace gozba_na_klik_backend.Services.DTOs
{
    public class PublicRestaurantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public string Address { get; set; } = string.Empty;
        public List<RestaurantWorkingHoursDTO> WorkingHours { get; set; } = new();
        public List<NonWorkingDayDTO> NonWorkingDays { get; set; } = new();
    }
}
