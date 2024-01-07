using Microsoft.AspNetCore.Mvc;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Notification")]
    public class NotificationController : Controller
    {
        [HttpGet("My")]
        public IActionResult MyNotification()
        {
            return View();
        }
    }
}
