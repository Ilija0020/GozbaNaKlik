using gozba_na_klik_backend.Services.DTOs;
using Microsoft.AspNetCore.Http;

namespace gozba_na_klik_backend.Services.Interfaces
{
    public interface IMealService
    {
        Task<List<MealDTO>> GetMealsByRestaurantIdAsync(int restaurantId);
        Task<MealDTO> CreateMealAsync(string ownerId, int restaurantId, MealCreateDTO mealDto);
        Task<MealDTO> UpdateMealAsync(string ownerId, int restaurantId, int mealId, MealUpdateDTO mealDto);
        Task DeleteMealAsync(string ownerId, int restaurantId, int mealId);
        Task<List<AllergenDTO>> GetAllAllergensAsync();
        Task<MealDTO> UploadMealImageAsync(string ownerId, int restaurantId, int mealId, IFormFile image);
    }
}
