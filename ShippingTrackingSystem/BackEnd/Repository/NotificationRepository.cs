using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models.Context;
using ShippingTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MyDbContext _context;

        public NotificationRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                return await _context.Notifications
                       .Where(n => n.UserId == userId && !n.IsDeleted)
                       .OrderByDescending(n => n.CreatedOn)
                       .ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Notification>();
            }
           
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            try
            {
                return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && !n.IsDeleted);
            }
            catch (Exception)
            {
                return new Notification();
            }
            
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    notification.IsDeleted = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            try
            {
                return await _context.Notifications
                                 .Where(n => n.UserId == userId && !n.IsRead && !n.IsDeleted)
                                 .CountAsync();
            }
            catch (Exception)
            {
                return 0;
            }            
        }
    }
}
