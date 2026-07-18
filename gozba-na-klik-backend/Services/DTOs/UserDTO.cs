
namespace gozba_na_klik_backend.Services.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public bool IsSuspended { get; set; }
        public int? WorkplaceRestaurantId { get; set; }
    }
}