using gozba_na_klik_backend.Domain.Entities;

namespace gozba_na_klik_backend.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<List<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task AddRestaurantAsync(Restaurant restaurant);
        Task UpdateRestaurantAsync(Restaurant restaurant);
        Task DeleteRestaurantAsync(Restaurant restaurant);
        Task<List<Restaurant>> GetRestaurantsByOwnerIdAsync(int ownerId);
        Task ReplaceWorkingHoursAsync(int restaurantId, List<RestaurantWorkingHours> newHours);
        Task ReplaceNonWorkingDaysAsync(int restaurantId, List<NonWorkingDay> newDays);
    }
}
