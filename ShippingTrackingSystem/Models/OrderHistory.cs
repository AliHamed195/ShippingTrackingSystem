using ShippingTrackingSystem.Enum;
using System.ComponentModel.DataAnnotations;

namespace ShippingTrackingSystem.Models
{
    public class OrderHistory
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime StatusChangedOn { get; set; } = DateTime.Now;

        public string EditorUserId { get; set; }
        public ApplicationUser EditorUser { get; set; }

        public string? Notes { get; set; }
    }
}
