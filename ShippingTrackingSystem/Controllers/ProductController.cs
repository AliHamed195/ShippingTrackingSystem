using Microsoft.AspNetCore.Mvc;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
