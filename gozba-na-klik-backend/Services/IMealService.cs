using gozba_na_klik_backend.DTOs;

namespace gozba_na_klik_backend.Services
{
    public interface IMealService
    {
        Task<List<MealDTO>> GetMealsByRestaurantIdAsync(int ownerId, int restaurantId);
        Task<MealDTO?> CreateMealAsync(int ownerId, int restaurantId, MealCreateDTO mealDto);
        Task<MealDTO?> UpdateMealAsync(int ownerId, int restaurantId, int mealId, MealUpdateDTO mealDto);
        Task<bool> DeleteMealAsync(int ownerId, int restaurantId, int mealId);
        Task<List<AllergenDTO>> GetAllAllergensAsync();
        Task<MealDTO?> UploadMealImageAsync(int ownerId, int restaurantId, int mealId, IFormFile image);
    }
}
