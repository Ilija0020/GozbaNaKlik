using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Services;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> RegisterUserAsync([FromBody] User newUser)
        {
            try
            {
                string errorMessage = await _userService.RegisterUserAsync(newUser);
                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                return Ok("Registracija uspesna.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Doslo je do greske na serveru. Molimo pokusajte ponovo kasnije.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] User loginData)
        {
            try
            {
                User user = await _userService.AuthenticateUserAsync(loginData.Username, loginData.Password);
                if (user == null)
                {
                    return BadRequest("Neispravno korisnicko ime ili lozinka.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Doslo je do greske na serveru. Molimo pokusajte ponovo kasnije.");
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterByAdmin([FromBody] User newUser)
        {
            try
            {
                string errorMessage = await _userService.RegisterUserAsync(newUser, true);
                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                return Ok("Korisnik uspesno registrovan.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Doslo je do greske na serveru. Molimo pokusajte ponovo kasnije.");
            }
        }
    }
}
