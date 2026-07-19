using gozba_na_klik_backend.Services.DTOs;
using System.Security.Claims;

namespace gozba_na_klik_backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(UserRegisterDTO newUserDto);
        Task<string> AuthenticateUserAsync(string userName, string password);
        Task<UserDTO> GetProfileAsync(ClaimsPrincipal userPrincipal);
    }
}
