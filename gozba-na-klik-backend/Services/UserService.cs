using AutoMapper;
using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Exceptions;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.Enums;

namespace gozba_na_klik_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task ValidateNewUserAsync(string username, string email)
        {
            User? existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
                throw new BadRequestException("Korisnicko ime je vec zauzeto.");

            User? existingEmail = await _userRepository.GetUserByEmailAsync(email);
            if (existingEmail != null)
                throw new BadRequestException("Email adresa je vec registrovana.");
        }

        public async Task RegisterUserAsync(UserRegisterDTO newUserDto)
        {
            await ValidateNewUserAsync(newUserDto.Username, newUserDto.Email);

            User newUser = _mapper.Map<User>(newUserDto);
            newUser.Role = Role.Customer;

            await _userRepository.AddUserAsync(newUser);
            _logger.LogInformation("Korisnik {Username} se registrovao.", newUser.Username);
        }

        public async Task RegisterUserByAdminAsync(UserAdminRegisterDTO newUserDto)
        {
            await ValidateNewUserAsync(newUserDto.Username, newUserDto.Email);

            User newUser = _mapper.Map<User>(newUserDto);

            await _userRepository.AddUserAsync(newUser);
            _logger.LogInformation("Administrator je registrovao korisnika {Username} sa ulogom {Role}.", newUser.Username, newUser.Role);
        }

        public async Task<UserDTO> AuthenticateUserAsync(string username, string password)
        {
            User? user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password)
                throw new BadRequestException("Neispravno korisnicko ime ili lozinka.");

            _logger.LogInformation("Korisnik {Username} se prijavio.", username);
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