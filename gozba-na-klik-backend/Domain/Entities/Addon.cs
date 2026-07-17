using gozba_na_klik_backend.Domain.Enums;

namespace gozba_na_klik_backend.Domain.Entities
{
    public class Addon
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required TypeOfAddon TypeOfAddon { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? GroupName { get; set; }
        public int? MealId { get; set; }
    }
}
