using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurantsAsync()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurantAsync([FromBody] RestaurantCreateDTO restaurantDto)
        {
            await _restaurantService.CreateRestaurantAsync(restaurantDto);
            return Ok("Restoran je uspesno kreiran.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurantAsync(int id, [FromBody] RestaurantUpdateDTO restaurantDto)
        {
            await _restaurantService.UpdateRestaurantAsync(id, restaurantDto);
            return Ok("Restoran je uspesno izmenjen.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantAsync(int id)
        {
            await _restaurantService.DeleteRestaurantAsync(id);
            return Ok("Restoran je uspesno obrisan.");
        }

        [HttpGet("owners/{ownerId}")]
        public async Task<IActionResult> GetRestaurantsByOwnerId(int ownerId)
        {
            var restaurants = await _restaurantService.GetRestaurantsByOwnerIdAsync(ownerId);
            return Ok(restaurants);
        }

        [HttpPut("owners/{ownerId}/restaurants/{id}")]
        public async Task<IActionResult> UpdateRestaurantByOwnerAsync(int id, int ownerId, [FromBody] RestaurantOwnerUpdateDTO restaurantDto)
        {
            await _restaurantService.UpdateRestaurantByOwnerAsync(id, ownerId, restaurantDto);
            return Ok("Restoran je uspesno izmenjen.");
        }

        [HttpPost("owners/{ownerId}/restaurants/{id}/upload-photo")]
        public async Task<IActionResult> UploadImageAsync(int id, int ownerId, IFormFile photo)
        {
            await _restaurantService.UploadRestaurantImageAsync(id, ownerId, photo);
            return Ok("Slika je uspesno uploadovana.");
        }

        [HttpPut("owners/{ownerId}/restaurants/{id}/working-hours")]
        public async Task<IActionResult> UpdateWorkingHoursAsync(int id, int ownerId, [FromBody] List<RestaurantWorkingHoursDTO> newHoursDto)
        {
            await _restaurantService.UpdateWorkingHoursAsync(id, ownerId, newHoursDto);
            return Ok("Radno vreme je uspesno izmenjeno.");
        }

        [HttpPut("owners/{ownerId}/restaurants/{id}/non-working-days")]
        public async Task<IActionResult> UpdateNonWorkingDaysAsync(int id, int ownerId, [FromBody] List<NonWorkingDayDTO> newNonWorkingDaysDto)
        {
            await _restaurantService.UpdateNonWorkingDaysAsync(id, ownerId, newNonWorkingDaysDto);
            return Ok("Neradni dani su uspesno izmenjeni.");
        }
    }
}