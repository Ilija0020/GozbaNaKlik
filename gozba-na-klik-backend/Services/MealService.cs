using AutoMapper;
using gozba_na_klik_backend.DTOs;
using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.IRepositories;

namespace gozba_na_klik_backend.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly IMapper _mapper;

        public MealService(IMealRepository mealRepository, IRestaurantsRepository restaurantsRepository, IMapper mapper)
        {
            _mealRepository = mealRepository;
            _restaurantsRepository = restaurantsRepository;
            _mapper = mapper;
        }

        public async Task<MealDTO?> CreateMealAsync(int ownerId, int restaurantId, MealCreateDTO mealDto)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            Menu? menu = await _mealRepository.GetMenuByRestaurantIdAsync(restaurantId);
            if (menu == null)
            {
                return null;
            }

            List<int> uniqueAllergenIds = mealDto.AllergenIds.Distinct().ToList(); // da nemamo duplikate alergena slucajno
            List<Allergen> allergens = await _mealRepository.GetAllergensByIdsAsync(uniqueAllergenIds);
            if (allergens.Count != uniqueAllergenIds.Count)
            {
                return null;
            }

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
            return _mapper.Map<MealDTO>(meal);
        }

        public async Task<bool> DeleteMealAsync(int ownerId, int restaurantId, int mealId)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null || restaurant.OwnerId != ownerId)
            {
                return false;
            }

            Meal? meal = await _mealRepository.GetMealByIdAsync(mealId);
            if (meal == null || meal.Menu.RestaurantId != restaurantId)
            {
                return false;
            }

            await _mealRepository.DeleteMealAsync(meal);
            return true;
        }

        public async Task<List<AllergenDTO>> GetAllAllergensAsync()
        {
            List<Allergen> allergens = await _mealRepository.GetAllAllergensAsync();
            return _mapper.Map<List<AllergenDTO>>(allergens);
        }

        public async Task<List<MealDTO>> GetMealsByRestaurantIdAsync(int ownerId, int restaurantId)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);

            if (restaurant == null || restaurant.OwnerId != ownerId)
            {
                return new List<MealDTO>();
            }

            List<Meal> meals = await _mealRepository.GetMealsByRestaurantIdAsync(restaurantId);

            return _mapper.Map<List<MealDTO>>(meals);
        }

        public async Task<MealDTO?> UpdateMealAsync(int ownerId, int restaurantId, int mealId, MealUpdateDTO mealDto)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            Meal? meal = await _mealRepository.GetMealByIdAsync(mealId);
            if (meal == null || meal.Menu.RestaurantId != restaurantId)
            {
                return null;
            }

            List<int> uniqueAllergenIds = mealDto.AllergenIds.Distinct().ToList(); // da nemamo duplikate alergena slucajno
            List<Allergen> allergens = await _mealRepository.GetAllergensByIdsAsync(uniqueAllergenIds);
            if (allergens.Count != uniqueAllergenIds.Count)
            {
                return null;
            }

            meal.Name = mealDto.Name;
            meal.Description = mealDto.Description;
            meal.Price = mealDto.Price;
            meal.Allergens = allergens;

            await _mealRepository.UpdateMealAsync(meal);
            return _mapper.Map<MealDTO>(meal);
        }

        public async Task<MealDTO?> UploadMealImageAsync(int ownerId, int restaurantId, int mealId, IFormFile image)
        {
            Restaurant? restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            Meal? meal = await _mealRepository.GetMealByIdAsync(mealId);
            if (meal == null || meal.Menu.RestaurantId != restaurantId)
            {
                return null;
            }

            if (image == null || image.Length == 0)
            {
                return null;
            }

            long maxFileSize = 5 * 1024 * 1024; // 5 MB

            if (image.Length > maxFileSize)
            {
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(meal.Photo))
            {
                string existingFilePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    meal.Photo.TrimStart('/'));

                if (File.Exists(existingFilePath))
                {
                    File.Delete(existingFilePath);
                }
            }

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

            meal.Photo = "/images/" + uniqueFileName;

            await _mealRepository.UpdateMealAsync(meal);

            return _mapper.Map<MealDTO>(meal);
        }
    }
}
