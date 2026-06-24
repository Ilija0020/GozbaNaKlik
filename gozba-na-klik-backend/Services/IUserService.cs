using gozba_na_klik_backend.Models;

namespace gozba_na_klik_backend.Services
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(User newUser, bool isFromAdmin = false);
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetAllOwnersAsync();
    }
}