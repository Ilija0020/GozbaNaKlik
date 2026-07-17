namespace gozba_na_klik_backend.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public bool Delivery { get; set; }
        public string? DeliveryAddress { get; set; }
        public int RestaurantId { get; set; }
        public required Restaurant Restaurant { get; set; }
        public int CourierId { get; set; }
        public User? Courier { get; set; }
        public int CustomerId { get; set; }
        public required User Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
