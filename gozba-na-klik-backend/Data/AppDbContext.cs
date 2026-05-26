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

            // --- 1. VLASNICI (OWNERS) ---
            // Koristimo veće ID-jeve (npr. od 100 pa na gore) da ne bi došlo do konflikta sa tvojim već postojećim podacima (poput admina).
            modelBuilder.Entity<User>().HasData(
                new User { Id = 101, Name = "Petar", Surname = "Petrovic", Email = "petar@vlasnik.com", Username = "petar_v", Password = "123", Role = Role.Owner },
                new User { Id = 102, Name = "Marko", Surname = "Markovic", Email = "marko@vlasnik.com", Username = "marko_v", Password = "123", Role = Role.Owner },
                new User { Id = 103, Name = "Jovan", Surname = "Jovanovic", Email = "jovan@vlasnik.com", Username = "jovan_v", Password = "123", Role = Role.Owner }
            );

            // --- 2. RESTORANI ---
            // Svaki vlasnik dobija po 2 restorana
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { Id = 101, Name = "Restoran Kod Petra", Address = "Glavna 10, Novi Sad", OwnerId = 101 },
                new Restaurant { Id = 102, Name = "Petrova Picerija", Address = "Dunavska 5, Novi Sad", OwnerId = 101 },

                new Restaurant { Id = 103, Name = "Markova Kafana", Address = "Zmaj Jovina 12, Novi Sad", OwnerId = 102 },
                new Restaurant { Id = 104, Name = "Marko Grill", Address = "Bulevar Oslobodjenja 45, Novi Sad", OwnerId = 102 },

                new Restaurant { Id = 105, Name = "Jovanov Restoran", Address = "Laze Teleckog 3, Novi Sad", OwnerId = 103 },
                new Restaurant { Id = 106, Name = "Pekara Jovan", Address = "Futoski put 10, Novi Sad", OwnerId = 103 }
            );

            // --- 3. JELOVNICI (MENUS) ---
            // Svaki restoran ima po 1 jelovnik
            modelBuilder.Entity<Menu>().HasData(
                new Menu { Id = 101, RestaurantId = 101, Restaurant = null!, Meals = null! },
                new Menu { Id = 102, RestaurantId = 102, Restaurant = null!, Meals = null! },
                new Menu { Id = 103, RestaurantId = 103, Restaurant = null!, Meals = null! },
                new Menu { Id = 104, RestaurantId = 104, Restaurant = null!, Meals = null! },
                new Menu { Id = 105, RestaurantId = 105, Restaurant = null!, Meals = null! },
                new Menu { Id = 106, RestaurantId = 106, Restaurant = null!, Meals = null! }
            );

            // --- 4. JELA (MEALS) ---
            // Za svaki jelovnik dodajemo po 4 jela (ukupno 24)
            modelBuilder.Entity<Meal>().HasData(
                // Jela za Restoran Kod Petra (Meni 101)
                new Meal { Id = 101, Name = "Cevapi", Description = "10 komada sa lukom", Price = 800, MenuId = 101, Menu = null! },
                new Meal { Id = 102, Name = "Pljeskavica", Description = "Gurmanska 250g", Price = 700, MenuId = 101, Menu = null! },
                new Meal { Id = 103, Name = "Sopska Salata", Description = "Paradajz, krastavac, sir", Price = 350, MenuId = 101, Menu = null! },
                new Meal { Id = 104, Name = "Teleca Corba", Description = "Domaca corba", Price = 400, MenuId = 101, Menu = null! },

                // Jela za Petrova Picerija (Meni 102)
                new Meal { Id = 105, Name = "Kapricoza", Description = "Sunka, sir, pecurke", Price = 900, MenuId = 102, Menu = null! },
                new Meal { Id = 106, Name = "Margarita", Description = "Pelat, sir, masline", Price = 750, MenuId = 102, Menu = null! },
                new Meal { Id = 107, Name = "Madjarica", Description = "Kulen, ljuta paprika", Price = 950, MenuId = 102, Menu = null! },
                new Meal { Id = 108, Name = "Pica Hleb", Description = "Beli luk, maslinovo ulje", Price = 250, MenuId = 102, Menu = null! },

                // Jela za Markova Kafana (Meni 103)
                new Meal { Id = 109, Name = "Karadjordjeva", Description = "Pohovano meso, kajmak", Price = 1200, MenuId = 103, Menu = null! },
                new Meal { Id = 110, Name = "Svadbarski Kupus", Description = "Kupus, suvo meso", Price = 850, MenuId = 103, Menu = null! },
                new Meal { Id = 111, Name = "Pohovani Kackavalj", Description = "Uz tartar sos", Price = 600, MenuId = 103, Menu = null! },
                new Meal { Id = 112, Name = "Domaca Pogaca", Description = "Topla pecena", Price = 200, MenuId = 103, Menu = null! },

                // Jela za Marko Grill (Meni 104)
                new Meal { Id = 113, Name = "Batak na zaru", Description = "Otkoštani batak 300g", Price = 650, MenuId = 104, Menu = null! },
                new Meal { Id = 114, Name = "Belo Meso", Description = "Piletina na zaru", Price = 600, MenuId = 104, Menu = null! },
                new Meal { Id = 115, Name = "Kobasice", Description = "Domace kobasice 300g", Price = 750, MenuId = 104, Menu = null! },
                new Meal { Id = 116, Name = "Pomfrit", Description = "Sveze przen", Price = 250, MenuId = 104, Menu = null! },

                // Jela za Jovanov Restoran (Meni 105)
                new Meal { Id = 117, Name = "Gulas", Description = "Govedji gulas", Price = 900, MenuId = 105, Menu = null! },
                new Meal { Id = 118, Name = "Pire Krompir", Description = "Kao prilog", Price = 200, MenuId = 105, Menu = null! },
                new Meal { Id = 119, Name = "Pohovana Piletina", Description = "U susamu", Price = 700, MenuId = 105, Menu = null! },
                new Meal { Id = 120, Name = "Krempita", Description = "Domaci desert", Price = 300, MenuId = 105, Menu = null! },

                // Jela za Pekara Jovan (Meni 106)
                new Meal { Id = 121, Name = "Burek sa Mesom", Description = "250g", Price = 220, MenuId = 106, Menu = null! },
                new Meal { Id = 122, Name = "Burek sa Sirom", Description = "250g", Price = 200, MenuId = 106, Menu = null! },
                new Meal { Id = 123, Name = "Kroasan Cokolada", Description = "Mali kroasan", Price = 120, MenuId = 106, Menu = null! },
                new Meal { Id = 124, Name = "Jogurt", Description = "Casa 0.25l", Price = 60, MenuId = 106, Menu = null! }
            );
        }
    }
}
