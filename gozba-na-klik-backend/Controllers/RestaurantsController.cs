using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Services;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly RestaurantService _restaurantService;

        public RestaurantsController(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurantsAsync()
        {
            try
            {
                var restaurants = await _restaurantService.GetAllRestaurantsAsync();

                var result = restaurants.Select(r => new
                {
                    id = r.Id,
                    name = r.Name,
                    address = r.Address,
                    ownerId = r.OwnerId
                });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurantAsync([FromBody] Restaurant restaurant)
        {
            try
            {
                string error = await _restaurantService.CreateRestaurantAsync(restaurant);
                if (!string.IsNullOrEmpty(error))
                    return BadRequest(error);

                return Ok("Restoran je uspesno kreiran.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurantAsync(int id, [FromBody] Restaurant restaurant)
        {
            try
            {
                string error = await _restaurantService.UpdateRestaurantAsync(id, restaurant);
                if (!string.IsNullOrEmpty(error))
                    return BadRequest(error);

                return Ok("Restoran je uspesno izmenjen.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantAsync(int id)
        {
            try
            {
                string error = await _restaurantService.DeleteRestaurantAsync(id);
                if (!string.IsNullOrEmpty(error))
                    return BadRequest(error);

                return Ok("Restoran je uspesno obrisan.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        //Owner specific
        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetRestaurantsByOwnerId(int ownerId)
        {
            try
            {
                var restaurants = await _restaurantService.GetRestaurantsByOwnerIdAsync(ownerId);
                return Ok(restaurants);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPut("owners/{ownerId}/restaurants/{id}")]
        public async Task<IActionResult> UpdateRestaurantByOwnerAsync(int id, int ownerId, [FromBody] Restaurant restaurant)
        {
            try
            {
                string error = await _restaurantService.UpdateRestaurantByOwnerAsync(id, ownerId, restaurant);
                if (error != "")
                    return BadRequest(error);

                return Ok("Restoran je uspesno izmenjen.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPost("owners/{ownerId}/restaurants/{id}/upload-photo")]
        public async Task<IActionResult> UploadImageAsync(int id, int ownerId, IFormFile photo)
        {
            try
            {
                string error = await _restaurantService.UploadRestaurantImageAsync(id, ownerId, photo);
                if (error != "")
                    return BadRequest(error);

                return Ok("Slika je uspesno uploadovana.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPut("owners/{ownerId}/restaurants/{id}/working-hours")]
        public async Task<IActionResult> UpdateWorkingHoursAsync(int id, int ownerId, [FromBody] List<RestaurantWorkingHours> newHours)
        {
            try
            {
                string error = await _restaurantService.UpdateWorkingHoursAsync(id, ownerId, newHours);
                if (error != "")
                    return BadRequest(error);
                return Ok("Radno vreme je uspesno izmenjeno.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPut("owners/{ownerId}/restaurants/{id}/non-working-days")]
        public async Task<IActionResult> UpdateNonWorkingDaysAsync(int id, int ownerId, [FromBody] List<NonWorkingDay> newNonWorkingDays)
        {
            try
            {
                string error = await _restaurantService.UpdateNonWorkingDaysAsync(id, ownerId, newNonWorkingDays);
                if (error != "")
                    return BadRequest(error);
                return Ok("Neradni dani su uspesno izmenjeni.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }
    }
}
