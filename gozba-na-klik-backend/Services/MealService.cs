using AutoMapper;
using gozba_na_klik_backend.Services.Interfaces;
using gozba_na_klik_backend.Services.DTOs;
using gozba_na_klik_backend.Services.Exceptions;
using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Domain.Repositories;

namespace gozba_na_klik_backend.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MealService> _logger;

        public MealService(IMealRepository mealRepository, IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<MealService> logger)
        {
            _mealRepository = mealRepository;
            _restaurantsRepository = restaurantsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MealDTO> CreateMealAsync(string ownerId, int restaurantId, MealCreateDTO mealDto)
        {
            await GetRestaurantForOwnerAsync(ownerId, restaurantId);
            Menu menu = await GetMenuByRestaurantIdAsync(restaurantId);

            List<Allergen> allergens = await GetValidAllergensAsync(mealDto.AllergenIds);

            Meal meal = new Meal
            {
                Name = mealDto.Name,
                Description = mealDto.Description,
                Price = mealDto.Price,
                MenuId = menu.Id,
                Menu = menu,
                Allergens = allergens
            };

            await _mealRepository.AddMealAsync(meal);

            _logger.LogInformation("Vlasnik {OwnerId} je dodao jelo {MealName} za restoran {RestaurantId}.", ownerId, meal.Name, restaurantId);

            return _mapper.Map<MealDTO>(meal);
        }

        public async Task DeleteMealAsync(string ownerId, int restaurantId, int mealId)
        {
            await GetRestaurantForOwnerAsync(ownerId, restaurantId);
            Meal meal = await GetMealForRestaurantAsync(restaurantId, mealId);

            await _mealRepository.DeleteMealAsync(meal);

            _logger.LogInformation("Vlasnik {OwnerId} je obrisao jelo {MealId} za restoran {RestaurantId}.", ownerId, mealId, restaurantId);
        }

        public async Task<List<AllergenDTO>> GetAllAllergensAsync()
        {
            List<Allergen> allergens = await _mealRepository.GetAllAllergensAsync();
            return _mapper.Map<List<AllergenDTO>>(allergens);
        }

        public async Task<List<MealDTO>> GetMealsByRestaurantIdAsync(int restaurantId)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException("Restoran nije pronadjen.");
            }

            List<Meal> meals = await _mealRepository.GetMealsByRestaurantIdAsync(restaurantId);

            return _mapper.Map<List<MealDTO>>(meals);
        }

        public async Task<MealDTO> UpdateMealAsync(string ownerId, int restaurantId, int mealId, MealUpdateDTO mealDto)
        {
            await GetRestaurantForOwnerAsync(ownerId, restaurantId);

            Meal meal = await GetMealForRestaurantAsync(restaurantId, mealId);

            List<Allergen> allergens = await GetValidAllergensAsync(mealDto.AllergenIds);

            meal.Name = mealDto.Name;
            meal.Description = mealDto.Description;
            meal.Price = mealDto.Price;
            meal.Allergens = allergens;

            await _mealRepository.UpdateMealAsync(meal);

            _logger.LogInformation("Vlasnik {OwnerId} je izmenio jelo {MealId} za restoran {RestaurantId}.", ownerId, mealId, restaurantId);

            return _mapper.Map<MealDTO>(meal);
        }

        public async Task<MealDTO> UploadMealImageAsync(string ownerId, int restaurantId, int mealId, IFormFile image)
        {
            await GetRestaurantForOwnerAsync(ownerId, restaurantId);

            Meal meal = await GetMealForRestaurantAsync(restaurantId, mealId);

            ValidateMealImage(image);

            DeleteMealImageIfExists(meal.Photo);

            meal.Photo = await SaveMealImageAsync(image);

            await _mealRepository.UpdateMealAsync(meal);

            _logger.LogInformation("Vlasnik {OwnerId} je uploadovao sliku za jelo {MealId}.", ownerId, mealId);

            return _mapper.Map<MealDTO>(meal);
        }

        private async Task<Restaurant> GetRestaurantForOwnerAsync(string ownerId, int restaurantId)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException("Restoran nije pronadjen.");
            }
            if (restaurant.OwnerId != ownerId)
            {
                throw new ForbiddenException("Nemate dozvolu za upravljanje jelima ovog restorana.");
            }
            return restaurant;
        }

        private async Task<Menu> GetMenuByRestaurantIdAsync(int restaurantId)
        {
            Menu? menu = await _mealRepository.GetMenuByRestaurantIdAsync(restaurantId);
            if (menu == null)
            {
                throw new NotFoundException("Jelovnik za ovaj restoran nije pronadjen.");
            }

            return menu;
        }

        private async Task<Meal> GetMealForRestaurantAsync(int restaurantId, int mealId)
        {
            Meal? meal = await _mealRepository.GetMealByIdAsync(mealId);
            if (meal == null)
            {
                throw new NotFoundException("Jelo nije pronadjeno.");
            }
            if (meal.Menu.RestaurantId != restaurantId)
            {
                throw new BadRequestException("Jelo ne pripada izabranom restoranu.");
            }

            return meal;
        }

        private async Task<List<Allergen>> GetValidAllergensAsync(List<int> allergenIds)
        {
            List<int> uniqueAllergenIds = allergenIds.Distinct().ToList(); // da nemamo duplikate alergena slucajno
            List<Allergen> allergens = await _mealRepository.GetAllergensByIdsAsync(uniqueAllergenIds);
            if (allergens.Count != uniqueAllergenIds.Count)
            {
                throw new BadRequestException("Jedan ili vise alergena nije pronadjeno.");
            }

            return allergens;
        }

        private static void ValidateMealImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new BadRequestException("Morate poslati sliku jela.");
            }

            long maxFileSize = 5 * 1024 * 1024; // 5 MB

            if (image.Length > maxFileSize)
            {
                throw new BadRequestException("Fajl je prevelik. Maksimalna dozvoljena velicina je 5 MB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new BadRequestException("Nepodrzan format fajla. Dozvoljeni formati su: .jpg, .jpeg, .png, .webp.");
            }
        }

        private static async Task<string> SaveMealImageAsync(IFormFile image)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

        private static void DeleteMealImageIfExists(string? photoPath)
        {
            if (string.IsNullOrEmpty(photoPath))
            {
                return;
            }

            string existingFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                photoPath.TrimStart('/'));

            if (File.Exists(existingFilePath))
            {
                File.Delete(existingFilePath);
            }
        }
    }
}
