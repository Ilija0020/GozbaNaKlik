namespace gozba_na_klik_backend.Domain.Entities
{
    public class CourierWorkingHours
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
