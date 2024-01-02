using ShippingTrackingSystem.Enum;

namespace ShippingTrackingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime? EstimatedDeliveryDate { get; set; } = DateTime.Now.AddDays(3);
    }
}
