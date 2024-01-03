namespace ShippingTrackingSystem.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
