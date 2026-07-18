using AutoMapper;
using gozba_na_klik_backend.Services.Interfaces;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik_backend.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RegisterUserByAdminAsync(UserAdminRegisterDTO newUserDto)
        {
            ApplicationUser newUser = _mapper.Map<ApplicationUser>(newUserDto);

            IdentityResult createResult = await _userManager.CreateAsync(newUser, newUserDto.Password);
            if (!createResult.Succeeded)
            {
                string errors = string.Join(";", createResult.Errors.Select(error => error.Description));

                throw new BadRequestException(errors);
            }

            string roleName = newUserDto.Role;

            IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, roleName);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);

                string errors = string.Join(";", roleResult.Errors.Select(error => error.Description));

                throw new BadRequestException(errors);
            }
            _logger.LogInformation("Administrator je registrovao korisnika {Username} sa ulogom {Role}.", newUser.UserName, roleName);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            List<ApplicationUser> users = await _userManager.Users.ToListAsync();

            List<UserDTO> userDtos = new List<UserDTO>();
            foreach (ApplicationUser user in users)
            {
                UserDTO userDto = await MapUserToDtoAsync(user);
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        public async Task<List<UserDTO>> GetAllOwnersAsync()
        {
            IList<ApplicationUser> owners = await _userManager.GetUsersInRoleAsync("Owner");

            List<UserDTO> ownerDtos = new List<UserDTO>();

            foreach (ApplicationUser owner in owners)
            {
                UserDTO ownerDto = await MapUserToDtoAsync(owner);
                ownerDtos.Add(ownerDto);
            }

            return ownerDtos;

        }

        private async Task<UserDTO> MapUserToDtoAsync(ApplicationUser user)
        {
            UserDTO userDto = _mapper.Map<UserDTO>(user);

            IList<string> roles =
                await _userManager.GetRolesAsync(user);

            if (roles.Count != 1)
            {
                throw new InvalidOperationException($"Korisnik '{user.UserName}' nema ispravno podesenu ulogu.");
            }

            userDto.Role = roles[0];

            return userDto;
        }
    }
}
