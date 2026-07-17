using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik_backend.Services.DTOs
{
    public class MealCreateDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public required string Description { get; set; }
        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }
        public List<int> AllergenIds { get; set; } = new List<int>();
    }
}
