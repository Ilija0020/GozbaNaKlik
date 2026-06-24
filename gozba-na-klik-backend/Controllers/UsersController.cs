using gozba_na_klik_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpGet("owners")]
        public async Task<IActionResult> GetAllOwnersAsync()
        {
            try
            {
                var owners = await _userService.GetAllOwnersAsync();
                return Ok(owners);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }
    }
}