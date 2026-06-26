using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealsController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet("allergens")]
        public async Task<IActionResult> GetAllAllergensAsync()
        {
            try
            {
                var allergens = await _mealService.GetAllAllergensAsync();
                return Ok(allergens);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpGet("owners/{ownerId}/restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> GetMealsByRestaurantIdAsync(int ownerId, int restaurantId)
        {
            try
            {
                var meals = await _mealService.GetMealsByRestaurantIdAsync(ownerId, restaurantId);
                return Ok(meals);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPost("owners/{ownerId}/restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> CreateMealAsync(int ownerId, int restaurantId, [FromBody] MealCreateDTO mealDto)
        {
            try
            {
                var meal = await _mealService.CreateMealAsync(ownerId, restaurantId, mealDto);

                if (meal == null)
                {
                    return BadRequest("Jelo nije moguce kreirati.");
                }
                return Ok(meal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPut("owners/{ownerId}/restaurants/{restaurantId}/meals/{mealId}")]
        public async Task<IActionResult> UpdateMealAsync(int ownerId, int restaurantId, int mealId, [FromBody] MealUpdateDTO mealDto)
        {
            try
            {
                var meal = await _mealService.UpdateMealAsync(ownerId, restaurantId, mealId, mealDto);

                if (meal == null)
                {
                    return BadRequest("Jelo nije moguce izmeniti.");
                }

                return Ok(meal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpDelete("owners/{ownerId}/restaurants/{restaurantId}/meals/{mealId}")]
        public async Task<IActionResult> DeleteMealAsync(int ownerId, int restaurantId, int mealId)
        {
            try
            {
                bool isDeleted = await _mealService.DeleteMealAsync(ownerId, restaurantId, mealId);

                if (!isDeleted)
                {
                    return BadRequest("Jelo nije moguce obrisati.");
                }
                return Ok("Jelo je uspesno obrisano.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }

        [HttpPost("owners/{ownerId}/restaurants/{restaurantId}/meals/{mealId}/upload-photo")]
        public async Task<IActionResult> UploadMealImageAsync(int ownerId, int restaurantId, int mealId, IFormFile photo)
        {
            try
            {
                var meal = await _mealService.UploadMealImageAsync(ownerId, restaurantId, mealId, photo);
                if (meal == null)
                {
                    return BadRequest("Sliku jela nije moguce uploadovati.");
                }
                return Ok(meal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Doslo je do greske na serveru.");
            }
        }
    }
}
