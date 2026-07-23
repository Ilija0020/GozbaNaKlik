using gozba_na_klik_backend.Domain.Enums;
using gozba_na_klik_backend.Domain.Queries;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("paging")]
        public async Task<IActionResult> GetRestaurantPageAsync(
            [FromQuery] RestaurantFilter filter,
            [FromQuery] int sortType = (int)RestaurantSortType.NameAscending,
            [FromQuery] int page = 1)
        {
            if (page < 1)
            {
                return BadRequest("Broj stranice mora biti veci od nule.");
            }

            return Ok(await _restaurantService.GetAllRestaurantsPagedAsync(filter, sortType, page));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublicRestaurantByIdAsync(int id)
        {
            PublicRestaurantDTO restaurant =
                await _restaurantService.GetPublicRestaurantByIdAsync(id);

            return Ok(restaurant);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRestaurantAsync([FromBody] RestaurantCreateDTO restaurantDto)
        {
            await _restaurantService.CreateRestaurantAsync(restaurantDto);
            return Ok("Restoran je uspesno kreiran.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurantAsync(int id, [FromBody] RestaurantUpdateDTO restaurantDto)
        {
            await _restaurantService.UpdateRestaurantAsync(id, restaurantDto);
            return Ok("Restoran je uspesno izmenjen.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantAsync(int id)
        {
            await _restaurantService.DeleteRestaurantAsync(id);
            return Ok("Restoran je uspesno obrisan.");
        }
    }
}
