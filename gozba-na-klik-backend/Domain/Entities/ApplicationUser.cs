using Microsoft.AspNetCore.Identity;

namespace gozba_na_klik_backend.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public bool IsDeleted { get; set; } = false;
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
