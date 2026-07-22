using gozba_na_klik_backend.Domain.Entities;
using gozba_na_klik_backend.Infrastructure.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik_backend.Infrastructure.Identity
{
    public static class DevelopmentSeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            var petar = await CreateOwnerAsync(
                userManager,
                "petar_v",
                "petar@vlasnik.com",
                "Petar",
                "Petrovic");

            var marko = await CreateOwnerAsync(
                userManager,
                "marko_v",
                "marko@vlasnik.com",
                "Marko",
                "Markovic");

            var jovan = await CreateOwnerAsync(
                userManager,
                "jovan_v",
                "jovan@vlasnik.com",
                "Jovan",
                "Jovanovic");

            await CreateRestaurantAsync(
                context,
                101,
                "Restoran Kod Petra",
                "Tradicionalna jela sa rostilja i domaca kuhinja.",
                "Glavna 10, Novi Sad",
                petar.Id,
                new[]
                {
                    (101, "Cevapi", "10 komada sa lukom", 800m),
                    (102, "Pljeskavica", "Gurmanska 250g", 700m),
                    (103, "Sopska Salata", "Paradajz, krastavac, sir", 350m),
                    (104, "Teleca Corba", "Domaca corba", 400m)
                });

            await CreateRestaurantAsync(
                context,
                102,
                "Petrova Picerija",
                "Pice iz peci pripremljene po domacem receptu.",
                "Dunavska 5, Novi Sad",
                petar.Id,
                new[]
                {
                    (105, "Kapricoza", "Sunka, sir, pecurke", 900m),
                    (106, "Margarita", "Pelat, sir, masline", 750m),
                    (107, "Madjarica", "Kulen, ljuta paprika", 950m),
                    (108, "Pica Hleb", "Beli luk, maslinovo ulje", 250m)
                });

            await CreateRestaurantAsync(
                context,
                103,
                "Markova Kafana",
                "Kafanska atmosfera i provereni domaci specijaliteti.",
                "Zmaj Jovina 12, Novi Sad",
                marko.Id,
                new[]
                {
                    (109, "Karadjordjeva", "Pohovano meso, kajmak", 1200m),
                    (110, "Svadbarski Kupus", "Kupus, suvo meso", 850m),
                    (111, "Pohovani Kackavalj", "Uz tartar sos", 600m),
                    (112, "Domaca Pogaca", "Topla pecena", 200m)
                });

            await CreateRestaurantAsync(
                context,
                104,
                "Marko Grill",
                "Jela sa rostilja pripremljena po porudzbini.",
                "Bulevar Oslobodjenja 45, Novi Sad",
                marko.Id,
                new[]
                {
                    (113, "Batak na zaru", "Otkostani batak 300g", 650m),
                    (114, "Belo Meso", "Piletina na zaru", 600m),
                    (115, "Kobasice", "Domace kobasice 300g", 750m),
                    (116, "Pomfrit", "Sveze przen", 250m)
                });

            await CreateRestaurantAsync(
                context,
                105,
                "Jovanov Restoran",
                "Raznovrsna kuvana jela i domaci deserti.",
                "Laze Teleckog 3, Novi Sad",
                jovan.Id,
                new[]
                {
                    (117, "Gulas", "Govedji gulas", 900m),
                    (118, "Pire Krompir", "Kao prilog", 200m),
                    (119, "Pohovana Piletina", "U susamu", 700m),
                    (120, "Krempita", "Domaci desert", 300m)
                });

            await CreateRestaurantAsync(
                context,
                106,
                "Pekara Jovan",
                "Sveza peciva, burek i jogurt tokom celog dana.",
                "Futoski put 10, Novi Sad",
                jovan.Id,
                new[]
                {
                    (121, "Burek sa Mesom", "250g", 220m),
                    (122, "Burek sa Sirom", "250g", 200m),
                    (123, "Kroasan Cokolada", "Mali kroasan", 120m),
                    (124, "Jogurt", "Casa 0.25l", 60m)
                });

            await context.SaveChangesAsync();

            await AddMealAllergensAsync(context);

            await context.SaveChangesAsync();
        }

        private static async Task AddMealAllergensAsync(AppDbContext context)
        {
            Dictionary<int, int[]> mealAllergenIds = new()
            {
                { 102, new[] { 1, 2 } },
                { 103, new[] { 2 } },
                { 104, new[] { 1, 9 } },

                { 105, new[] { 1, 2 } },
                { 106, new[] { 1, 2 } },
                { 107, new[] { 1, 2 } },
                { 108, new[] { 1 } },

                { 109, new[] { 1, 2, 5 } },
                { 110, new[] { 9 } },
                { 111, new[] { 1, 2, 5 } },
                { 112, new[] { 1 } },

                { 115, new[] { 10 } },

                { 117, new[] { 9 } },
                { 118, new[] { 2 } },
                { 119, new[] { 1, 5, 11 } },
                { 120, new[] { 1, 2, 5 } },

                { 121, new[] { 1 } },
                { 122, new[] { 1, 2 } },
                { 123, new[] { 1, 2, 5 } },
                { 124, new[] { 2 } }
            };

            Dictionary<int, Meal> meals = await context.Meals
                .Where(meal => mealAllergenIds.Keys.Contains(meal.Id))
                .Include(meal => meal.Allergens)
                .ToDictionaryAsync(meal => meal.Id);

            Dictionary<int, Allergen> allergens = await context.Allergens
                .ToDictionaryAsync(allergen => allergen.Id);

            foreach (KeyValuePair<int, int[]> mealAllergen in mealAllergenIds)
            {
                if (!meals.TryGetValue(mealAllergen.Key, out Meal? meal))
                {
                    continue;
                }

                meal.Allergens ??= new List<Allergen>();

                foreach (int allergenId in mealAllergen.Value)
                {
                    if (!allergens.TryGetValue(allergenId, out Allergen? allergen))
                    {
                        continue;
                    }

                    bool alreadyAdded = meal.Allergens.Any(
                        existingAllergen => existingAllergen.Id == allergenId);

                    if (!alreadyAdded)
                    {
                        meal.Allergens.Add(allergen);
                    }
                }
            }
        }

        private static async Task<ApplicationUser> CreateOwnerAsync(
            UserManager<ApplicationUser> userManager,
            string username,
            string email,
            string name,
            string surname)
        {
            var owner = await userManager.FindByNameAsync(username);

            if (owner == null)
            {
                owner = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    Name = name,
                    Surname = surname,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(owner, "Owner123!");
                EnsureSucceeded(createResult, $"Kreiranje korisnika {username}");
            }

            if (!await userManager.IsInRoleAsync(owner, "Owner"))
            {
                var roleResult = await userManager.AddToRoleAsync(owner, "Owner");
                EnsureSucceeded(roleResult, $"Dodeljivanje Owner role korisniku {username}");
            }

            return owner;
        }

        private static async Task CreateRestaurantAsync(
            AppDbContext context,
            int id,
            string name,
            string description,
            string address,
            string ownerId,
            IEnumerable<(int Id, string Name, string Description, decimal Price)> meals)
        {
            if (await context.Restaurants.AnyAsync(r => r.Id == id || r.Name == name))
            {
                return;
            }

            var restaurant = new Restaurant
            {
                Id = id,
                Name = name,
                Description = description,
                Address = address,
                OwnerId = ownerId
            };

            var menu = new Menu
            {
                Id = id,
                RestaurantId = id,
                Restaurant = restaurant,
                Meals = new List<Meal>()
            };

            restaurant.Menu = menu;

            foreach (var mealData in meals)
            {
                menu.Meals.Add(new Meal
                {
                    Id = mealData.Id,
                    Name = mealData.Name,
                    Description = mealData.Description,
                    Price = mealData.Price,
                    MenuId = menu.Id,
                    Menu = menu
                });
            }

            context.Restaurants.Add(restaurant);
        }

        private static void EnsureSucceeded(IdentityResult result, string operation)
        {
            if (result.Succeeded)
            {
                return;
            }

            var errors = string.Join(", ", result.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"{operation} nije uspelo: {errors}");
        }
    }
}
