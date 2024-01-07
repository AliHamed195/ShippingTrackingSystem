namespace ShippingTrackingSystem.ViewModels
{
    public class UserOrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
