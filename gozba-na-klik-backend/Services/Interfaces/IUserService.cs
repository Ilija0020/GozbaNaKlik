using gozba_na_klik_backend.Services.DTOs;

namespace gozba_na_klik_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserByAdminAsync(UserAdminRegisterDTO newUserDto);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<UserDTO>> GetAllOwnersAsync();
    }
}
