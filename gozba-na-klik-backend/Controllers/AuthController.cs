using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterDTO newUserDto)
        {
            await _userService.RegisterUserAsync(newUserDto);
            return Ok("Registracija uspesna.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginDTO loginData)
        {
            UserDTO user = await _userService.AuthenticateUserAsync(loginData.Username, loginData.Password);
            return Ok(user);
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterByAdmin([FromBody] UserAdminRegisterDTO newUserDto)
        {
            await _userService.RegisterUserByAdminAsync(newUserDto);
            return Ok("Korisnik uspesno registrovan.");
        }
    }
}