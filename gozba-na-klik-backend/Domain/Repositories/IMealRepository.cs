using gozba_na_klik_backend.Domain.Entities;

namespace gozba_na_klik_backend.Domain.Repositories
{
    public interface IMealRepository
    {
        Task<List<Meal>> GetMealsByRestaurantIdAsync(int restaurantId);
        Task<Meal?> GetMealByIdAsync(int mealId);
        Task<Menu?> GetMenuByRestaurantIdAsync(int restaurantId);
        Task<List<Allergen>> GetAllergensByIdsAsync(List<int> allergenIds);
        Task<List<Allergen>> GetAllAllergensAsync();
        Task AddMealAsync(Meal meal);
        Task UpdateMealAsync(Meal meal);
        Task DeleteMealAsync(Meal meal);
    }
}
