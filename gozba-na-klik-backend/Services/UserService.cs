using AutoMapper;
using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.Enums;

namespace gozba_na_klik_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        private async Task<string> ValidateNewUserAsync(string username, string email)
        {
            User? existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
                return "Korisnicko ime je vec zauzeto.";

            User? existingEmail = await _userRepository.GetUserByEmailAsync(email);
            if (existingEmail != null)
                return "Email adresa je vec registrovana.";

            return "";
        }

        public async Task<string> RegisterUserAsync(UserRegisterDTO newUserDto)
        {
            string validationError = await ValidateNewUserAsync(newUserDto.Username, newUserDto.Email);
            if (validationError != "")
                return validationError;

            User newUser = _mapper.Map<User>(newUserDto);
            newUser.Role = Role.Customer;

            await _userRepository.AddUserAsync(newUser);
            return "";
        }

        public async Task<string> RegisterUserByAdminAsync(UserAdminRegisterDTO newUserDto)
        {
            string validationError = await ValidateNewUserAsync(newUserDto.Username, newUserDto.Email);
            if (validationError != "")
                return validationError;

            User newUser = _mapper.Map<User>(newUserDto);

            await _userRepository.AddUserAsync(newUser);
            return "";
        }

        public async Task<UserDTO?> AuthenticateUserAsync(string username, string password)
        {
            User? user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password)
                return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetAllOwnersAsync()
        {
            List<User> owners = await _userRepository.GetOwners();
            return _mapper.Map<List<UserDTO>>(owners);
        }
    }
}