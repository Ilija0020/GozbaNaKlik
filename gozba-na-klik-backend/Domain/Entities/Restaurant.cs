namespace gozba_na_klik_backend.Domain.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public required string Address { get; set; }
        public required string OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
        public Menu? Menu { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Order>? Orders { get; set; } = new List<Order>();
        public ICollection<ApplicationUser>? Employees { get; set; } = new List<ApplicationUser>();
        public ICollection<RestaurantWorkingHours>? WorkingHours { get; set; } = new List<RestaurantWorkingHours>();
        public ICollection<NonWorkingDay>? NonWorkingDays { get; set; } = new List<NonWorkingDay>();
        //Posto nam je jelo vec povezano sa restoranom preko menija, ne treba nam dodatna veza izmedju jela i restorana
        //public ICollection<Meal>? Meals { get; set; } = new List<Meal>();

    }
}
