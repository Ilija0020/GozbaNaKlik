using AutoMapper;
using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gozba_na_klik_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
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

        public async Task<string> AuthenticateUserAsync(string userName, string password)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new BadRequestException("Neispravno korisnicko ime ili lozinka.");
            }

            bool passwordMatches = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordMatches)
            {
                throw new BadRequestException("Neispravno korisnicko ime ili lozinka.");
            }

            string token = await GenerateJwt(user);

            _logger.LogInformation("Korisnik {Username} se prijavio.", user.UserName);

            return token;
        }

        public async Task<UserDTO> GetProfileAsync(ClaimsPrincipal userPrincipal)
        {
            string? userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new BadRequestException("Token ne sadrzi identifikator korisnika.");
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BadRequestException("Korisnik iz tokena vise ne postoji.");
            }

            UserDTO userDto = _mapper.Map<UserDTO>(user);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Count != 1)
            {
                throw new InvalidOperationException("Korisnik nema ispravno podesenu ulogu.");
            }

            userDto.Role = roles[0];
            return userDto;
        }

        private async Task<string> GenerateJwt(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),

                new Claim("username", user.UserName ?? string.Empty),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);

            if (roles.Count != 1)
            {
                throw new InvalidOperationException("Korisnik nema ispravno podesenu ulogu.");
            }

            claims.AddRange(roles.Select(role => new Claim("role", role)));

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            SigningCredentials credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
