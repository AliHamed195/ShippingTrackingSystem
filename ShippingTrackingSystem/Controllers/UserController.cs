using Microsoft.AspNetCore.Mvc;

namespace ShippingTrackingSystem.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
