using AutoMapper;
using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace gozba_na_klik_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RegisterUserAsync(UserRegisterDTO newUserDto)
        {
            ApplicationUser newUser = _mapper.Map<ApplicationUser>(newUserDto);

            IdentityResult createResult = await _userManager.CreateAsync(newUser, newUserDto.Password);

            if (!createResult.Succeeded)
            {
                string errors = string.Join(";", createResult.Errors.Select(error => error.Description));

                throw new BadRequestException(errors);
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, "Customer");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);

                string errors = string.Join(";", roleResult.Errors.Select(error => error.Description));

                throw new BadRequestException(errors);
            }
            _logger.LogInformation("Korisnik {Username} se registrovao.", newUser.UserName);
        }

        public async Task<UserDTO> AuthenticateUserAsync(string username, string password)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new BadRequestException("Neispravno korisnicko ime ili lozinka.");
            }

            bool passwordMatches = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordMatches)
            {
                throw new BadRequestException("Neispravno korisnicko ime ili lozinka.");
            }

            UserDTO userDto = _mapper.Map<UserDTO>(user);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Count != 1)
            {
                throw new InvalidOperationException("Korisnik nema ispravno podesenu ulogu.");
            }

            userDto.Role = roles[0];

            _logger.LogInformation("Korisnik {Username} se prijavio.", user.UserName);

            return userDto;
        }
    }
}
