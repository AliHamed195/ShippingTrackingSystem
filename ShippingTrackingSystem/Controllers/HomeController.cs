using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;
using System.Diagnostics;

namespace ShippingTrackingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> HomePage()
        {
            var (success, errorMessage, categories) = await _categoryRepository.GetAllCategoriesAsync();
            if (success)
            {
                return View(categories);
            }

            ModelState.AddModelError("", errorMessage);
            return View(Enumerable.Empty<Category>());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
