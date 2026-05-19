using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Repositories;

namespace gozba_na_klik_backend.Services
{
    public class RestaurantService
    {
        private readonly RestaurantRepository _restaurantRepository;

        public RestaurantService(RestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllRestaurantsAsync();
        }

        public async Task<string> CreateRestaurantAsync(Restaurant restaurant)
        {
            await _restaurantRepository.AddRestaurantAsync(restaurant);
            return "";
        }

        public async Task<string> UpdateRestaurantAsync(int id, Restaurant updatedData)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";

            restaurant.Name = updatedData.Name;
            restaurant.Address = updatedData.Address;
            restaurant.OwnerId = updatedData.OwnerId;

            await _restaurantRepository.UpdateRestaurantAsync(restaurant);
            return "";
        }

        public async Task<string> DeleteRestaurantAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";

            await _restaurantRepository.DeleteRestaurantAsync(restaurant);
            return "";
        }
    }
}
