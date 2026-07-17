namespace gozba_na_klik_backend.Domain.Entities
{
    public class RestaurantWorkingHours
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool EndsNextDay { get; set; }
        public int? RestaurantId { get; set; }
    }
}
