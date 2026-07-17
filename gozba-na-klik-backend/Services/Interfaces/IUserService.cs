using gozba_na_klik_backend.Services.DTOs;

namespace gozba_na_klik_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserRegisterDTO newUserDto);
        Task RegisterUserByAdminAsync(UserAdminRegisterDTO newUserDto);
        Task<UserDTO> AuthenticateUserAsync(string username, string password);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<UserDTO>> GetAllOwnersAsync();
    }
}
