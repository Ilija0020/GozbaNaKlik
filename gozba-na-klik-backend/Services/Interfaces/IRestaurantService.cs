using gozba_na_klik_backend.Services.DTOs;
using Microsoft.AspNetCore.Http;

namespace gozba_na_klik_backend.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDTO>> GetAllRestaurantsAsync();
        Task CreateRestaurantAsync(RestaurantCreateDTO restaurantDto);
        Task UpdateRestaurantAsync(int id, RestaurantUpdateDTO restaurantDto);
        Task DeleteRestaurantAsync(int id);
        Task<List<RestaurantDTO>> GetRestaurantsByOwnerIdAsync(int ownerId);
        Task UpdateRestaurantByOwnerAsync(int id, int ownerId, RestaurantOwnerUpdateDTO restaurantDto);
        Task UploadRestaurantImageAsync(int id, int ownerId, IFormFile image);
        Task UpdateWorkingHoursAsync(int id, int ownerId, List<RestaurantWorkingHoursDTO> newHoursDto);
        Task UpdateNonWorkingDaysAsync(int id, int ownerId, List<NonWorkingDayDTO> newNonWorkingDaysDto);
    }
}
