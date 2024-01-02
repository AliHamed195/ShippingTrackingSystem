using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;

namespace ShippingTrackingSystem.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public UserController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("All")]
        public IActionResult AllUsers()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpGet("Edit")]
        public IActionResult EditUser()
        {
            return View();
        }

        [HttpGet("Details")]
        public IActionResult UserDetails()
        {
            return View();
        }

        [HttpGet("Delete")]
        public IActionResult DeleteUser()
        {
            return View();
        }
    }
}
