namespace ShippingTrackingSystem.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
