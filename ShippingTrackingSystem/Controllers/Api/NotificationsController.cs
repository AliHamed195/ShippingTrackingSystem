using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;
using System.Security.Claims;

namespace ShippingTrackingSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // GET: api/Notifications/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        // GET: api/Notifications/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        // PUT: api/Notifications/MarkAsRead/{id}
        [HttpPut("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            var result = await _notificationRepository.MarkNotificationAsReadAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        // DELETE: api/Notifications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var result = await _notificationRepository.DeleteNotificationAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("UnreadCount")]
        public async Task<IActionResult> GetUnreadNotificationCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var count = await _notificationRepository.GetUnreadNotificationCountAsync(userId);
            return Ok(count);
        }
    }
}
