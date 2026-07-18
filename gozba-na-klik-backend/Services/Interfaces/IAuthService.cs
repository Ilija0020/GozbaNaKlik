using gozba_na_klik_backend.Services.DTOs;

namespace gozba_na_klik_backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(UserRegisterDTO newUserDto);
        Task<UserDTO> AuthenticateUserAsync(string username, string password);
    }
}
