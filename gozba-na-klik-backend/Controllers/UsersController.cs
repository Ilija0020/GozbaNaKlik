using gozba_na_klik_backend.Services;
using Microsoft.AspNetCore.Http;
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

                var result = users.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    surname = u.Surname,
                    email = u.Email,
                    username = u.Username,
                    role = u.Role
                });

                return Ok(result);
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

                var result = owners.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    surname = u.Surname,
                    username = u.Username,
                    role = u.Role
                });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }
    }
}
