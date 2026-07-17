namespace gozba_na_klik_backend.Domain.Entities
{
    public class NonWorkingDay
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int RestaurantId { get; set; }
    }
}
