using AutoMapper;
using gozba_na_klik_backend.Services.Interfaces;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace gozba_na_klik_backend.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantsRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(IRestaurantsRepository restaurantRepository, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RestaurantDTO>> GetAllRestaurantsAsync()
        {
            List<Restaurant> restaurants = await _restaurantRepository.GetAllRestaurantsAsync();
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public async Task CreateRestaurantAsync(RestaurantCreateDTO restaurantDto)
        {
            Restaurant restaurant = _mapper.Map<Restaurant>(restaurantDto);

            restaurant.Menu = new Menu
            {
                Restaurant = restaurant,
                Meals = new List<Meal>()
            };
            await _restaurantRepository.AddRestaurantAsync(restaurant);
            _logger.LogInformation("Restoran {RestaurantName} je kreiran sa praznim jelovnikom (OwnerId={OwnerId}).", restaurant.Name, restaurant.OwnerId);
        }

        public async Task UpdateRestaurantAsync(int id, RestaurantUpdateDTO restaurantDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restoran nije pronadjen.");

            restaurant.Name = restaurantDto.Name;
            restaurant.Address = restaurantDto.Address;
            restaurant.OwnerId = restaurantDto.OwnerId;

            await _restaurantRepository.UpdateRestaurantAsync(restaurant);
            _logger.LogInformation("Restoran {RestaurantId} je izmenjen od strane administratora.", id);
        }

        public async Task DeleteRestaurantAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restoran nije pronadjen.");

            await _restaurantRepository.DeleteRestaurantAsync(restaurant);
            _logger.LogInformation("Restoran {RestaurantId} je obrisan.", id);
        }

        public async Task<List<RestaurantDTO>> GetRestaurantsByOwnerIdAsync(int ownerId)
        {
            List<Restaurant> restaurants = await _restaurantRepository.GetRestaurantsByOwnerIdAsync(ownerId);
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public async Task UpdateRestaurantByOwnerAsync(int id, int ownerId, RestaurantOwnerUpdateDTO restaurantDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restoran nije pronadjen.");

            if (restaurant.OwnerId != ownerId)
                throw new ForbiddenException("Nemate dozvolu za izmenu ovog restorana.");

            restaurant.Name = restaurantDto.Name;
            restaurant.Address = restaurantDto.Address;
            restaurant.Description = restaurantDto.Description;

            await _restaurantRepository.UpdateRestaurantAsync(restaurant);
            _logger.LogInformation("Vlasnik {OwnerId} je izmenio restoran {RestaurantId}.", ownerId, id);
        }

        public async Task UploadRestaurantImageAsync(int id, int ownerId, IFormFile image)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restoran nije pronadjen.");

            if (restaurant.OwnerId != ownerId)
                throw new ForbiddenException("Nemate dozvolu za izmenu ovog restorana.");

            if (image == null || image.Length == 0)
                throw new BadRequestException("You must provide an image route.");

            long maxFileSize = 5 * 1024 * 1024;
            if (image.Length > maxFileSize)
                throw new BadRequestException("Fajl je prevelik. Maksimalna dozvoljena velicina je 5 MB.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new BadRequestException("Dozvoljeni su samo .jpg, .jpeg, .png i .webp formati slika.");

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
            _logger.LogInformation("Slika restorana {RestaurantId} je azurirana od strane vlasnika {OwnerId}.", id, ownerId);
        }

        public async Task UpdateWorkingHoursAsync(int id, int ownerId, List<RestaurantWorkingHoursDTO> newHoursDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restoran nije pronadjen.");
            if (restaurant.OwnerId != ownerId)
                throw new ForbiddenException("Nemate dozvolu za izmenu ovog restorana.");

            foreach (var hours in newHoursDto)
            {
                if (!hours.EndsNextDay && hours.StartTime >= hours.EndTime)
                    throw new BadRequestException($"Nelogicno radno vreme za dan {hours.Day}. Kraj mora biti posle pocetka.");

                if (hours.EndsNextDay && hours.StartTime <= hours.EndTime)
                    throw new BadRequestException($"Nelogicno radno vreme za dan {hours.Day}. Ako se radno vreme zavrsava sutradan, kraj mora biti pre pocetka.");
            }

            var duplicateDays = newHoursDto.GroupBy(h => h.Day).Where(g => g.Count() > 1).Any();
            if (duplicateDays)
                throw new BadRequestException("Ne mozete uneti isti dan vise puta.");

            List<RestaurantWorkingHours> newHours = _mapper.Map<List<RestaurantWorkingHours>>(newHoursDto);

            await _restaurantRepository.ReplaceWorkingHoursAsync(id, newHours);
            _logger.LogInformation("Radno vreme restorana {RestaurantId} je azurirano od strane vlasnika {OwnerId}.", id, ownerId);
        }

        public async Task UpdateNonWorkingDaysAsync(int id, int ownerId, List<NonWorkingDayDTO> newNonWorkingDaysDto)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restoran nije pronadjen.");
            if (restaurant.OwnerId != ownerId)
                throw new ForbiddenException("Nemate dozvolu za izmenu ovog restorana.");

            var today = DateOnly.FromDateTime(DateTime.Now);
            List<NonWorkingDay> newNonWorkingDays = _mapper.Map<List<NonWorkingDay>>(newNonWorkingDaysDto);
            var validNonWorkingDays = newNonWorkingDays.Where(d => d.Date >= today).ToList();

            await _restaurantRepository.ReplaceNonWorkingDaysAsync(id, validNonWorkingDays);
            _logger.LogInformation("Neradni dani restorana {RestaurantId} su azurirani od strane vlasnika {OwnerId}.", id, ownerId);
        }
    }
}
