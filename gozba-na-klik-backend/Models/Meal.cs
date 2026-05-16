namespace gozba_na_klik_backend.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public string? Photo { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int MenuId { get; set; }
        public required Menu Menu { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Allergen>? Allergens { get; set; } = new List<Allergen>();
        public ICollection<Addon>? Addons { get; set; } = new List<Addon>();
    }
}
