namespace gozba_na_klik_backend.Models
{
    public class NonWorkingDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int RestaurantId { get; set; }
    }
}
