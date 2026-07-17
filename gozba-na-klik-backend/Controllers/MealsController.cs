using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Interfaces;
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
            var allergens = await _mealService.GetAllAllergensAsync();
            return Ok(allergens);
        }

        [HttpGet("owners/{ownerId}/restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> GetMealsByRestaurantIdAsync(int ownerId, int restaurantId)
        {
            var meals = await _mealService.GetMealsByRestaurantIdAsync(ownerId, restaurantId);
            return Ok(meals);
        }

        [HttpPost("owners/{ownerId}/restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> CreateMealAsync(int ownerId, int restaurantId, [FromBody] MealCreateDTO mealDto)
        {
            var meal = await _mealService.CreateMealAsync(ownerId, restaurantId, mealDto);
            return Ok(meal);
        }

        [HttpPut("owners/{ownerId}/restaurants/{restaurantId}/meals/{mealId}")]
        public async Task<IActionResult> UpdateMealAsync(int ownerId, int restaurantId, int mealId, [FromBody] MealUpdateDTO mealDto)
        {
            var meal = await _mealService.UpdateMealAsync(ownerId, restaurantId, mealId, mealDto);
            return Ok(meal);
        }

        [HttpDelete("owners/{ownerId}/restaurants/{restaurantId}/meals/{mealId}")]
        public async Task<IActionResult> DeleteMealAsync(int ownerId, int restaurantId, int mealId)
        {
            await _mealService.DeleteMealAsync(ownerId, restaurantId, mealId);
            return Ok("Jelo je uspesno obrisano.");
        }

        [HttpPost("owners/{ownerId}/restaurants/{restaurantId}/meals/{mealId}/upload-photo")]
        public async Task<IActionResult> UploadMealImageAsync(int ownerId, int restaurantId, int mealId, IFormFile photo)
        {
            var meal = await _mealService.UploadMealImageAsync(ownerId, restaurantId, mealId, photo);
            return Ok(meal);
        }
    }
}
