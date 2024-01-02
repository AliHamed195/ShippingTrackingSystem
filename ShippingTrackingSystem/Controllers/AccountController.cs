using Microsoft.AspNetCore.Mvc;

namespace ShippingTrackingSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
