using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Services;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantService _restaurantService;

        public RestaurantController(RestaurantService restaurantService)
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
    }
}
