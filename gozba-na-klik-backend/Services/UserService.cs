using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.Enums;
using gozba_na_klik_backend.Repositories;

namespace gozba_na_klik_backend.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> RegisterUserAsync(User newUser, bool isFromAdmin = false)
        {
            User existingUser = await _userRepository.GetUserByUsernameAsync(newUser.Username); 
            if (existingUser != null)
            {
                return "Korisnicko ime je vec zauzeto.";
            }
            User existingEmail = await _userRepository.GetUserByEmailAsync(newUser.Email);
            if (existingEmail != null)
            {
                return "Email adresa je vec registrovana.";
            }
            if (!isFromAdmin)
            {
                newUser.Role = Role.Customer;
            }
            await _userRepository.AddUserAsync(newUser);
            return "";
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            User? user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password)
            {
                return null;
            }
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
            {
                return await _userRepository.GetAllUsersAsync();
        }

        public async Task<List<User>> GetAllOwnersAsync()
        {
            return await _userRepository.GetOwners();
        }
    }
}
