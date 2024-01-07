using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Notification")]
    [Authorize]
    public class NotificationController : Controller
    {
        [HttpGet("My")]
        public IActionResult MyNotification()
        {
            return View();
        }
    }
}
