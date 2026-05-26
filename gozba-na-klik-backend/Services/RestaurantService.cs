using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Repositories;

namespace gozba_na_klik_backend.Services
{
    public class RestaurantService
    {
        private readonly RestaurantsRepository _restaurantRepository;

        public RestaurantService(RestaurantsRepository restaurantRepository)
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

        //Owner specific
        public async Task<List<Restaurant>> GetRestaurantsByOwnerIdAsync(int ownerId)
        {
            return await _restaurantRepository.GetRestaurantsByOwnerIdAsync(ownerId);
        }

        public async Task<string> UpdateRestaurantByOwnerAsync(int id, int ownerId, Restaurant updatedData)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";

            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            restaurant.Name = updatedData.Name;
            restaurant.Address = updatedData.Address;
            restaurant.Description = updatedData.Description;

            await _restaurantRepository.UpdateRestaurantAsync(restaurant);
            return "";
        }

        public async Task<string> UploadRestaurantImageAsync(int id, int ownerId, IFormFile image)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";
            
            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            // Logic for uploading the image goes here
            if (image == null || image.Length == 0)
                return "You must provide an image route.";

            long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (image.Length > maxFileSize)
                return "Fajl je prevelik. Maksimalna dozvoljena velicina je 5 MB.";

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return "Dozvoljeni su samo .jpg, .jpeg, .png i .webp formati slika.";

            if (!string.IsNullOrEmpty(restaurant.Photo))
            {
                string existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", restaurant.Photo.TrimStart('/'));
                if (File.Exists(existingFilePath))
                    File.Delete(existingFilePath);
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            restaurant.Photo = "/images/" + uniqueFileName;

            await _restaurantRepository.UpdateRestaurantAsync(restaurant);
            return "";
        }

        // Additional methods for managing restaurant working hours and non-working days 
        public async Task<string> UpdateWorkingHoursAsync(int id, int ownerId,List<RestaurantWorkingHours> newHours)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            
            if (restaurant == null)
                return "Restoran nije pronadjen.";
            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            foreach (var hours in newHours)
            {
                if (hours.StartTime >= hours.EndTime)
                    return $"Nelogicno radno vreme za dan {hours.Day}. Kraj mora biti posle pocetka.";
            }

            var duplicateDays = newHours.GroupBy(h => h.Day).Where(g => g.Count() > 1).Any();
            if (duplicateDays)
                return "Ne mozete uneti isti dan vise puta.";

            await _restaurantRepository.ReplaceWorkingHoursAsync(id, newHours);
            return "";
        }

        public async Task<string> UpdateNonWorkingDaysAsync(int id, int ownerId, List<NonWorkingDay> newNonWorkingDays)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            
            if (restaurant == null)
                return "Restoran nije pronadjen.";
            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            var today = DateOnly.FromDateTime(DateTime.Now);
            var validNonWorkingDays = newNonWorkingDays.Where(d => d.Date >= today).ToList();

            await _restaurantRepository.ReplaceNonWorkingDaysAsync(id, validNonWorkingDays);
            return "";
        }
    }
}
