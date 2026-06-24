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
            try
            {
                string errorMessage = await _userService.RegisterUserAsync(newUserDto);
                if (errorMessage != "")
                    return BadRequest(errorMessage);

                return Ok("Registracija uspesna.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru. Molimo pokusajte ponovo kasnije.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginDTO loginData)
        {
            try
            {
                UserDTO? user = await _userService.AuthenticateUserAsync(loginData.Username, loginData.Password);
                if (user == null)
                    return BadRequest("Neispravno korisnicko ime ili lozinka.");

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru. Molimo pokusajte ponovo kasnije.");
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterByAdmin([FromBody] UserAdminRegisterDTO newUserDto)
        {
            try
            {
                string errorMessage = await _userService.RegisterUserByAdminAsync(newUserDto);
                if (errorMessage != "")
                    return BadRequest(errorMessage);

                return Ok("Korisnik uspesno registrovan.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru. Molimo pokusajte ponovo kasnije.");
            }
        }
    }
}