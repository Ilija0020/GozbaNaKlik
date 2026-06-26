using gozba_na_klik_backend.DTOs;

namespace gozba_na_klik_backend.Services
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