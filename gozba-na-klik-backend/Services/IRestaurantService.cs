using gozba_na_klik_backend.DTOs;
using Microsoft.AspNetCore.Http;

namespace gozba_na_klik_backend.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDTO>> GetAllRestaurantsAsync();
        Task<string> CreateRestaurantAsync(RestaurantCreateDTO restaurantDto);
        Task<string> UpdateRestaurantAsync(int id, RestaurantUpdateDTO restaurantDto);
        Task<string> DeleteRestaurantAsync(int id);
        Task<List<RestaurantDTO>> GetRestaurantsByOwnerIdAsync(int ownerId);
        Task<string> UpdateRestaurantByOwnerAsync(int id, int ownerId, RestaurantOwnerUpdateDTO restaurantDto);
        Task<string> UploadRestaurantImageAsync(int id, int ownerId, IFormFile image);
        Task<string> UpdateWorkingHoursAsync(int id, int ownerId, List<RestaurantWorkingHoursDTO> newHoursDto);
        Task<string> UpdateNonWorkingDaysAsync(int id, int ownerId, List<NonWorkingDayDTO> newNonWorkingDaysDto);
    }
}