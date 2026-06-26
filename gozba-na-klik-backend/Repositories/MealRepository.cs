using gozba_na_klik_backend.Data;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik_backend.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly AppDbContext _context;

        public MealRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMealAsync(Meal meal)
        {
            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMealAsync(Meal meal)
        {
            meal.IsDeleted = true;
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Allergen>> GetAllAllergensAsync()
        {
            return await _context.Allergens
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<List<Allergen>> GetAllergensByIdsAsync(List<int> allergenIds)
        {
            return await _context.Allergens
                .Where(a => allergenIds.Contains(a.Id))
                .ToListAsync();
        }

        public async Task<Meal?> GetMealByIdAsync(int mealId)
        {
            IQueryable<Meal> query = _context.Meals
                .Where(m => m.Id == mealId && !m.IsDeleted)
                .Include(m => m.Allergens)
                .Include(m => m.Menu);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Meal>> GetMealsByRestaurantIdAsync(int restaurantId)
        {
            IQueryable<Meal> query = _context.Meals
                .Where(m => !m.IsDeleted && m.Menu.RestaurantId == restaurantId)
                .Include(m => m.Allergens)
                .Include(m => m.Menu)
                .OrderBy(m => m.Name);

            return await query.ToListAsync();
        }

        public async Task<Menu?> GetMenuByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Menus
                .FirstOrDefaultAsync(m => m.RestaurantId == restaurantId);
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
        }
    }
}
