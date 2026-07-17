namespace gozba_na_klik_backend.Services.DTOs
{
    public class RestaurantWorkingHoursDTO
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool EndsNextDay { get; set; }
    }
}
