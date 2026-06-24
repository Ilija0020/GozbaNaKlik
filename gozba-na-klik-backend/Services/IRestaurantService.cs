using gozba_na_klik_backend.Models;

namespace gozba_na_klik_backend.Services
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetAllRestaurantsAsync();
        Task<string> CreateRestaurantAsync(Restaurant restaurant);
        Task<string> UpdateRestaurantAsync(int id, Restaurant updatedData);
        Task<string> DeleteRestaurantAsync(int id);
        Task<List<Restaurant>> GetRestaurantsByOwnerIdAsync(int ownerId);
        Task<string> UpdateRestaurantByOwnerAsync(int id, int ownerId, Restaurant updatedData);
        Task<string> UploadRestaurantImageAsync(int id, int ownerId, IFormFile image);
        Task<string> UpdateWorkingHoursAsync(int id, int ownerId, List<RestaurantWorkingHours> newHours);
        Task<string> UpdateNonWorkingDaysAsync(int id, int ownerId, List<NonWorkingDay> newNonWorkingDays);
    }
}