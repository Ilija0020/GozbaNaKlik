using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterDTO newUserDto)
        {
            await _authService.RegisterUserAsync(newUserDto);
            return Ok("Registracija uspesna.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginDTO loginData)
        {
            UserDTO user = await _authService.AuthenticateUserAsync(loginData.Username, loginData.Password);
            return Ok(user);
        }
    }
}
