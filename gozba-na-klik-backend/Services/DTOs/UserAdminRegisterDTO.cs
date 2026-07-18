
namespace gozba_na_klik_backend.Services.DTOs
{
    public class UserAdminRegisterDTO
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}