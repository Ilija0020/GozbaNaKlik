using gozba_na_klik_backend.Domain.Common;
using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Domain.Queries;

namespace gozba_na_klik_backend.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<List<Restaurant>> GetAllRestaurantsAsync();
        Task<PaginatedList<Restaurant>> GetAllRestaurantsPagedAsync(
            RestaurantFilter filter, int sortType, int page);
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task AddRestaurantAsync(Restaurant restaurant);
        Task UpdateRestaurantAsync(Restaurant restaurant);
        Task DeleteRestaurantAsync(Restaurant restaurant);
        Task<List<Restaurant>> GetRestaurantsByOwnerIdAsync(string ownerId);
        Task ReplaceWorkingHoursAsync(int restaurantId, List<RestaurantWorkingHours> newHours);
        Task ReplaceNonWorkingDaysAsync(int restaurantId, List<NonWorkingDay> newDays);
    }
}
