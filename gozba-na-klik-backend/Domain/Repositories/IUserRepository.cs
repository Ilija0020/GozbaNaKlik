using gozba_na_klik_backend.Domain.Entities;

namespace gozba_na_klik_backend.Domain.Repositories
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
