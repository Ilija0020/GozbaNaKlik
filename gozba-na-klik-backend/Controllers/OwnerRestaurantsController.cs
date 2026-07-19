using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/owner/restaurants")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class OwnerRestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public OwnerRestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOwnedRestaurantsAsync()
        {
            string ownerId = GetCurrentOwnerId();

            var restaurants =
                await _restaurantService
                    .GetRestaurantsByOwnerIdAsync(ownerId);

            return Ok(restaurants);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurantAsync(int id, [FromBody] RestaurantOwnerUpdateDTO restaurantDto)
        {
            string ownerId = GetCurrentOwnerId();

            await _restaurantService.UpdateRestaurantByOwnerAsync(id, ownerId, restaurantDto);

            return Ok("Restoran je uspesno izmenjen.");
        }

        [HttpPost("{id}/upload-photo")]
        public async Task<IActionResult> UploadImageAsync(int id, IFormFile photo)
        {
            string ownerId = GetCurrentOwnerId();

            await _restaurantService.UploadRestaurantImageAsync(id, ownerId, photo);

            return Ok("Slika je uspesno uploadovana.");
        }

        [HttpPut("{id}/working-hours")]
        public async Task<IActionResult> UpdateWorkingHoursAsync(int id, [FromBody] List<RestaurantWorkingHoursDTO> newHoursDto)
        {
            string ownerId = GetCurrentOwnerId();

            await _restaurantService.UpdateWorkingHoursAsync(id, ownerId, newHoursDto);

            return Ok("Radno vreme je uspesno izmenjeno.");
        }

        [HttpPut("{id}/non-working-days")]
        public async Task<IActionResult> UpdateNonWorkingDaysAsync(int id, [FromBody] List<NonWorkingDayDTO> newNonWorkingDaysDto)
        {
            string ownerId = GetCurrentOwnerId();

            await _restaurantService.UpdateNonWorkingDaysAsync(id, ownerId, newNonWorkingDaysDto);

            return Ok("Neradni dani su uspesno izmenjeni.");
        }

        private string GetCurrentOwnerId()
        {
            string? ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ownerId == null)
            {
                throw new BadRequestException(
                    "Token ne sadrzi identifikator vlasnika.");
            }

            return ownerId;
        }
    }
}
