using gozba_na_klik_backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace gozba_na_klik_backend.Infrastructure.Identity
{
    public class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>();

            var admin1 = new ApplicationUser
            {
                UserName = "admin1",
                Email = "admin1@gozbanaklik.com",
                Name = "Admin1",
                Surname = "Admin1",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync("admin1") == null)
            {
                await userManager.CreateAsync(
                    admin1,
                    "Admin123!");

                await userManager.AddToRoleAsync(
                    admin1,
                    "Admin");
            }

            var admin2 = new ApplicationUser
            {
                UserName = "admin2",
                Email = "admin2@gozbanaklik.com",
                Name = "Admin2",
                Surname = "Admin2",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync("admin2") == null)
            {
                await userManager.CreateAsync(
                    admin2,
                    "Admin234!");

                await userManager.AddToRoleAsync(
                    admin2,
                    "Admin");
            }

            var admin3 = new ApplicationUser
            {
                UserName = "admin3",
                Email = "admin3@gozbanaklik.com",
                Name = "Admin3",
                Surname = "Admin3",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync("admin3") == null)
            {
                await userManager.CreateAsync(
                    admin3,
                    "Admin345!");

                await userManager.AddToRoleAsync(
                    admin3,
                    "Admin");
            }
        }
    }
}