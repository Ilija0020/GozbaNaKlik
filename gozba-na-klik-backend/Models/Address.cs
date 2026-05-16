namespace gozba_na_klik_backend.Models
{
    public class Address
    {
        public int Id { get; set; }
        public required string Street { get; set; }
        public int StreetNumber { get; set; }
        public int? Floor { get; set; }
        public required string City { get; set; }
        public int UserId { get; set; }
    }
}
