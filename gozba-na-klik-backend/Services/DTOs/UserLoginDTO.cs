namespace gozba_na_klik_backend.Services.DTOs
{
    public class UserLoginDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}