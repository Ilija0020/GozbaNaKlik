using gozba_na_klik_backend.Models;

namespace gozba_na_klik_backend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetOwners();
    }
}