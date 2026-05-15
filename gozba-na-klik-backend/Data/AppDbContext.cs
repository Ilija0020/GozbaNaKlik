using gozba_na_klik_backend.Models;
using gozba_na_klik_backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
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
            // Cuvanje enuma kao string
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Addon>()
                 .Property(a => a.TypeOfAddon)
                 .HasConversion<string>();

            modelBuilder.Entity<RestaurantWorkingHours>()
                .Property(w => w.Day)
                .HasConversion<string>();

            modelBuilder.Entity<CourierWorkingHours>()
                .Property(w => w.Day)
                .HasConversion<string>();

            // Unique constraint na Username i email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Vise na vise konfiguracije
            // User - Allergen
            modelBuilder.Entity<User>()
                .HasMany(u => u.Allergens)
                .WithMany("Users")
                .UsingEntity<Dictionary<string, object>>(
                "UserAllergens",
                j => j.HasOne<Allergen>().WithMany().HasForeignKey("AlergenId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
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
            modelBuilder.Entity<User>()
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

            //Seeding
            //Alergije koje se nalaze u bazi
            modelBuilder.Entity<Allergen>().HasData(
                new Allergen { Id = 1, Name = "Gluten" },
                new Allergen { Id = 2, Name = "Lactose" },
                new Allergen { Id = 3, Name = "Nuts" },
                new Allergen { Id = 4, Name = "Shellfish" },
                new Allergen { Id = 5, Name = "Eggs" }
            );
            //Admin nalozi
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "Admin1",
                Surname = "Admin1",
                Email = "admin1@gozba_na_klik.com",
                Username = "admin1",
                Password = "admin1",
                Role = Role.Admin
            },
            new User
            {
                Id = 2,
                Name = "Admin2",
                Surname = "Admin2",
                Email = "admin2@gozba_na_klik.com",
                Username = "admin2",
                Password = "admin2",
                Role = Role.Admin
            },
            new User
            {
                Id = 3,
                Name = "Admin3",
                Surname = "Admin3",
                Email = "admin3@gozba_na_klik.com",
                Username = "admin3",
                Password = "admin3",
                Role = Role.Admin
            }
            );
        }
    }
}
