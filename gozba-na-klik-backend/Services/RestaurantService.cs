using AutoMapper;
using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Models;
using Microsoft.AspNetCore.Http;

namespace gozba_na_klik_backend.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantsRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public RestaurantService(IRestaurantsRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<List<RestaurantDTO>> GetAllRestaurantsAsync()
        {
            List<Restaurant> restaurants = await _restaurantRepository.GetAllRestaurantsAsync();
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public async Task<string> CreateRestaurantAsync(RestaurantCreateDTO restaurantDto)
        {
            Restaurant restaurant = _mapper.Map<Restaurant>(restaurantDto);
            await _restaurantRepository.AddRestaurantAsync(restaurant);
            return "";
        }

        public async Task<string> UpdateRestaurantAsync(int id, RestaurantUpdateDTO restaurantDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";

            restaurant.Name = restaurantDto.Name;
            restaurant.Address = restaurantDto.Address;
            restaurant.OwnerId = restaurantDto.OwnerId;

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

        public async Task<List<RestaurantDTO>> GetRestaurantsByOwnerIdAsync(int ownerId)
        {
            List<Restaurant> restaurants = await _restaurantRepository.GetRestaurantsByOwnerIdAsync(ownerId);
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public async Task<string> UpdateRestaurantByOwnerAsync(int id, int ownerId, RestaurantOwnerUpdateDTO restaurantDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";

            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            restaurant.Name = restaurantDto.Name;
            restaurant.Address = restaurantDto.Address;
            restaurant.Description = restaurantDto.Description;

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

            if (image == null || image.Length == 0)
                return "You must provide an image route.";

            long maxFileSize = 5 * 1024 * 1024;
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

        public async Task<string> UpdateWorkingHoursAsync(int id, int ownerId, List<RestaurantWorkingHoursDTO> newHoursDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";
            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            foreach (var hours in newHoursDto)
            {
                if (hours.StartTime >= hours.EndTime)
                    return $"Nelogicno radno vreme za dan {hours.Day}. Kraj mora biti posle pocetka.";
            }

            var duplicateDays = newHoursDto.GroupBy(h => h.Day).Where(g => g.Count() > 1).Any();
            if (duplicateDays)
                return "Ne mozete uneti isti dan vise puta.";

            List<RestaurantWorkingHours> newHours = _mapper.Map<List<RestaurantWorkingHours>>(newHoursDto);

            await _restaurantRepository.ReplaceWorkingHoursAsync(id, newHours);
            return "";
        }

        public async Task<string> UpdateNonWorkingDaysAsync(int id, int ownerId, List<NonWorkingDayDTO> newNonWorkingDaysDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return "Restoran nije pronadjen.";
            if (restaurant.OwnerId != ownerId)
                return "Nemate dozvolu za izmenu ovog restorana.";

            var today = DateOnly.FromDateTime(DateTime.Now);
            List<NonWorkingDay> newNonWorkingDays = _mapper.Map<List<NonWorkingDay>>(newNonWorkingDaysDto);
            var validNonWorkingDays = newNonWorkingDays.Where(d => d.Date >= today).ToList();

            await _restaurantRepository.ReplaceNonWorkingDaysAsync(id, validNonWorkingDays);
            return "";
        }
    }
}