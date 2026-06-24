using gozba_na_klik_backend.Models.Enums;

namespace gozba_na_klik_backend.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string? Photo { get; set; }
        public bool IsSuspended { get; set; }
        public int? WorkplaceRestaurantId { get; set; }
    }
}