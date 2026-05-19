using gozba_na_klik_backend.Data;
using gozba_na_klik_backend.Models;
using Microsoft.EntityFrameworkCore;
using gozba_na_klik_backend.Models.Enums;

namespace gozba_na_klik_backend.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetOwners()
        {
            return await _context.Users
                .Where(u => u.Role == Role.Owner)
                .ToListAsync();
        }
    }
}
