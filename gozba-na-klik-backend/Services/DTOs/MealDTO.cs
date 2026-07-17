namespace gozba_na_klik_backend.Services.DTOs
{
    public class MealDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public string? Photo { get; set; }
        public List<AllergenDTO> Allergens { get; set; } = new List<AllergenDTO>();
    }
}
