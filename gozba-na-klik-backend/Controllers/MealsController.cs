using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gozba_na_klik_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealsController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet("meals/allergens")]
        public async Task<IActionResult> GetAllAllergensAsync()
        {
            var allergens = await _mealService.GetAllAllergensAsync();
            return Ok(allergens);
        }

        [HttpGet("restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> GetMealsByRestaurantIdAsync(int restaurantId)
        {
            var meals = await _mealService.GetMealsByRestaurantIdAsync(restaurantId);
            return Ok(meals);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> CreateMealAsync(int restaurantId, [FromBody] MealCreateDTO mealDto)
        {
            string ownerId = GetCurrentOwnerId();
            var meal = await _mealService.CreateMealAsync(ownerId, restaurantId, mealDto);
            return Ok(meal);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("restaurants/{restaurantId}/meals/{mealId}")]
        public async Task<IActionResult> UpdateMealAsync(int restaurantId, int mealId, [FromBody] MealUpdateDTO mealDto)
        {
            string ownerId = GetCurrentOwnerId();
            var meal = await _mealService.UpdateMealAsync(ownerId, restaurantId, mealId, mealDto);
            return Ok(meal);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("restaurants/{restaurantId}/meals/{mealId}")]
        public async Task<IActionResult> DeleteMealAsync(int restaurantId, int mealId)
        {
            string ownerId = GetCurrentOwnerId();
            await _mealService.DeleteMealAsync(ownerId, restaurantId, mealId);
            return Ok("Jelo je uspesno obrisano.");
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("restaurants/{restaurantId}/meals/{mealId}/upload-photo")]
        public async Task<IActionResult> UploadMealImageAsync(int restaurantId, int mealId, IFormFile photo)
        {
            string ownerId = GetCurrentOwnerId();
            var meal = await _mealService.UploadMealImageAsync(ownerId, restaurantId, mealId, photo);
            return Ok(meal);
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
