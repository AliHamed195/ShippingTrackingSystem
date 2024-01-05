using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShippingTrackingSystem.Enum;
using System.ComponentModel.DataAnnotations;

namespace ShippingTrackingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        [Display(Name = "Shipping Tracking Number")]
        [ValidateNever]
        public string TrackingNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime? EstimatedDeliveryDate { get; set; } = DateTime.Now.AddDays(3);

        public virtual ICollection<OrderDetail> OrdersDetails { get; set; }

        public virtual ICollection<OrderHistory>? OrderHistories { get; set; }
    }
}
