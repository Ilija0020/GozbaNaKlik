using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Interfaces;
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
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("owners")]
        public async Task<IActionResult> GetAllOwnersAsync()
        {
            var owners = await _userService.GetAllOwnersAsync();
            return Ok(owners);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterByAdmin([FromBody] UserAdminRegisterDTO newUserDto)
        {
            await _userService.RegisterUserByAdminAsync(newUserDto);
            return Ok("Korisnik uspesno registrovan.");
        }
    }
}
