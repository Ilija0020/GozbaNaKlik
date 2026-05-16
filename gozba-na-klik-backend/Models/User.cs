using gozba_na_klik_backend.Models.Enums;

namespace gozba_na_klik_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Role Role { get; set; }
        public string? Photo { get; set; }
        public bool IsSuspended { get; set; } = false;
        public int? WorkplaceRestaurantId { get; set; }
        public Restaurant? WorkplaceRestaurant { get; set; }
        public ICollection<CourierWorkingHours>? CourierWorkingHours { get; set; } = new List<CourierWorkingHours>();
        public ICollection<Address>? Addresses { get; set; } = new List<Address>();
        public ICollection<Allergen>? Allergens { get; set; } = new List<Allergen>();
        public ICollection<Restaurant>? OwnedRestaurants { get; set; } = new List<Restaurant>();


    }
}
