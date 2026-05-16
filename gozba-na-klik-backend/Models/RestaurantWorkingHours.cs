namespace gozba_na_klik_backend.Models
{
    public class RestaurantWorkingHours
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? RestaurantId { get; set; }
    }
}
