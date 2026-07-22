using gozba_na_klik_backend.Infrastructure.PostgreSql;
using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Domain.Repositories;
using gozba_na_klik_backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using gozba_na_klik_backend.Domain.Queries;
using gozba_na_klik_backend.Domain.Common;

namespace gozba_na_klik_backend.Infrastructure.PostgreSql.Repositories
{
    public class RestaurantsRepository : IRestaurantsRepository
    {
        private readonly AppDbContext _context;
        private const int PageSize = 9;

        public RestaurantsRepository(AppDbContext context)
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

        public async Task<PaginatedList<Restaurant>> GetAllRestaurantsPagedAsync(
            RestaurantFilter filter, int sortType, int page)
        {
            IQueryable<Restaurant> restaurants = _context.Restaurants
                .Where(restaurant => !restaurant.IsDeleted)
                .Include(restaurant => restaurant.WorkingHours)
                .Include(restaurant => restaurant.NonWorkingDays);

            restaurants = FilterRestaurants(restaurants, filter);
            restaurants = SortRestaurants(restaurants, sortType);

            int pageIndex = page - 1;
            int count = await restaurants.CountAsync();

            List<Restaurant> items = await restaurants
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToListAsync();

            PaginatedList<Restaurant> result = new PaginatedList<Restaurant>(
                items, count, pageIndex, PageSize);

            return result;
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

        // Methods for working with restaurant by owner        
        public async Task<List<Restaurant>> GetRestaurantsByOwnerIdAsync(string ownerId)
        {
            return await _context.Restaurants
                .Where(r => r.OwnerId == ownerId && !r.IsDeleted)
                .Include(r => r.WorkingHours)
                .Include(r => r.NonWorkingDays)
                .ToListAsync();
        }
        public async Task ReplaceWorkingHoursAsync(int restaurantId, List<RestaurantWorkingHours> newHours)
        {
            var oldHours = _context.Set<RestaurantWorkingHours>()
                .Where(h => h.RestaurantId == restaurantId);

            _context.Set<RestaurantWorkingHours>().RemoveRange(oldHours);

            foreach (var hours in newHours)
                hours.RestaurantId = restaurantId;

            _context.Set<RestaurantWorkingHours>().AddRange(newHours);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceNonWorkingDaysAsync(int restaurantId, List<NonWorkingDay> newDays)
        {
            var oldDays = _context.Set<NonWorkingDay>()
                .Where(d => d.RestaurantId == restaurantId);

            _context.Set<NonWorkingDay>().RemoveRange(oldDays);
            
            foreach (var day in newDays)
                day.RestaurantId = restaurantId;
            
            _context.Set<NonWorkingDay>().AddRange(newDays);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Restaurant> FilterRestaurants(
            IQueryable<Restaurant> restaurants, RestaurantFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                restaurants = restaurants.Where(restaurant =>
                    restaurant.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                restaurants = restaurants.Where(restaurant =>
                    restaurant.Address.ToLower().Contains(filter.Address.ToLower()));
            }

            return restaurants;
        }

        private static IQueryable<Restaurant> SortRestaurants(IQueryable<Restaurant> restaurants, int sortType)
        {
            return sortType switch
            {
                (int)RestaurantSortType.NameAscending =>
                    restaurants.OrderBy(restaurant => restaurant.Name),

                (int)RestaurantSortType.NameDescending =>
                    restaurants.OrderByDescending(restaurant => restaurant.Name),

                (int)RestaurantSortType.AddressAscending =>
                    restaurants.OrderBy(restaurant => restaurant.Address),

                (int)RestaurantSortType.AddressDescending =>
                    restaurants.OrderByDescending(restaurant => restaurant.Address),

                _ => restaurants.OrderBy(restaurant => restaurant.Name)
            };
        }
    }
}
