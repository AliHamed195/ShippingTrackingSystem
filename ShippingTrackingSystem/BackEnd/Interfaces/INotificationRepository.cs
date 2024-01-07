using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}
