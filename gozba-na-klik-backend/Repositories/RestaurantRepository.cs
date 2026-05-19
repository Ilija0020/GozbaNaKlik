using gozba_na_klik_backend.Data;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik_backend.Repositories
{
    public class RestaurantRepository
    {
        private readonly AppDbContext _context;

        public RestaurantRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync() 
        {
            return await _context.Restaurants
                .Where(r => !r.IsDeleted)
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            return await _context.Restaurants
                .Where(r => r.Id == id && !r.IsDeleted)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync();
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRestaurantAsync(Restaurant restaurant)
        {
            restaurant.IsDeleted = true;
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}
