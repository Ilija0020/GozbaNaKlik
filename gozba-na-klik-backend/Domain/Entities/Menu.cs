namespace gozba_na_klik_backend.Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public required Restaurant Restaurant { get; set; }
        public required ICollection<Meal> Meals { get; set; } = new List<Meal>();

    }
}
