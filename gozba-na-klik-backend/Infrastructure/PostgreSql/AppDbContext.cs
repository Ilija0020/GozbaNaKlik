using gozba_na_klik_backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik_backend.Infrastructure.PostgreSql
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<RestaurantWorkingHours> WorkingHours { get; set; }
        public DbSet<CourierWorkingHours> CourierWorkingHours { get; set; }
        public DbSet<NonWorkingDay> NonWorkingDays { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "role-admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "role-admin"
                },
                new IdentityRole
                {
                    Id = "role-customer",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "role-customer"
                },
                new IdentityRole
                {
                    Id = "role-owner",
                    Name = "Owner",
                    NormalizedName = "OWNER",
                    ConcurrencyStamp = "role-owner"
                },
                new IdentityRole
                {
                    Id = "role-employee",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "role-employee"
                },
                new IdentityRole
                {
                    Id = "role-courier",
                    Name = "Courier",
                    NormalizedName = "COURIER",
                    ConcurrencyStamp = "role-courier"
                }
            );

            modelBuilder.Entity<Allergen>().HasData(
                new Allergen { Id = 1, Name = "Cereals containing gluten" },
                new Allergen { Id = 2, Name = "Milk (including lactose)" },
                new Allergen { Id = 3, Name = "Nuts" },
                new Allergen { Id = 4, Name = "Crustaceans" },
                new Allergen { Id = 5, Name = "Eggs" },
                new Allergen { Id = 6, Name = "Fish" },
                new Allergen { Id = 7, Name = "Peanuts" },
                new Allergen { Id = 8, Name = "Soybeans" },
                new Allergen { Id = 9, Name = "Celery" },
                new Allergen { Id = 10, Name = "Mustard" },
                new Allergen { Id = 11, Name = "Sesame seeds" },
                new Allergen { Id = 12, Name = "Sulphur dioxide and sulphites" },
                new Allergen { Id = 13, Name = "Lupin" },
                new Allergen { Id = 14, Name = "Molluscs" }
            );

            modelBuilder.Entity<Addon>()
                 .Property(a => a.TypeOfAddon)
                 .HasConversion<string>();

            modelBuilder.Entity<RestaurantWorkingHours>()
                .Property(w => w.Day)
                .HasConversion<string>();

            modelBuilder.Entity<CourierWorkingHours>()
                .Property(w => w.Day)
                .HasConversion<string>();

            // Vise na vise konfiguracije
            // User - Allergen
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Allergens)
                .WithMany("Users")
                .UsingEntity<Dictionary<string, object>>(
                "UserAllergens",
                j => j.HasOne<Allergen>().WithMany().HasForeignKey("AlergenId"),
                j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId")
                );
            // Meal - Allergen
            modelBuilder.Entity<Meal>()
                .HasMany(m => m.Allergens)
                .WithMany("Meals")
                .UsingEntity<Dictionary<string, object>>(
                "MealAllergens",
                j => j.HasOne<Allergen>().WithMany().HasForeignKey("AlergenId"),
                j => j.HasOne<Meal>().WithMany().HasForeignKey("MealId")
                );
            //Zastita od brisanja
            //Radnik ostaje u bazi, ako je isDeleted true kod restorana
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.WorkplaceRestaurant)
                .WithMany(r => r.Employees)
                .HasForeignKey(u => u.WorkplaceRestaurantId)
                .OnDelete(DeleteBehavior.Restrict);
            //Vlasnik i restoran
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.OwnedRestaurants)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            //Porudzbina i kupac
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            // Porudzbina i kurir
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Courier)
                .WithMany()
                .HasForeignKey(o => o.CourierId)
                .OnDelete(DeleteBehavior.Restrict);
            // Zabrana brisanja jela ako se nalazi u OrderItem
            modelBuilder.Entity<Meal>()
                .HasMany(m => m.OrderItems)
                .WithOne(oi => oi.Meal)
                .HasForeignKey(oi => oi.MealId)
                .OnDelete(DeleteBehavior.Restrict);
            // Brisanje menija ako ima jela
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.Meals)
                .WithOne(me => me.Menu)
                .HasForeignKey(me => me.MenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
