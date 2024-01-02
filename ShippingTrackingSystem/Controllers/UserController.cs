using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public UserController(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpGet("All")]
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                var users = await _accountRepository.GetAllUsersAsync();
                return View(users);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                return View(Enumerable.Empty<ApplicationUser>());
            }
        }

        [HttpGet("Create")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(ApplicationUser newUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string defaultPassword = _configuration["DefaultPassword"];
                    var user = await _accountRepository.CreateUserAsync(newUser, defaultPassword);

                    if (user is not null)
                    {
                        return RedirectToAction(nameof(AllUsers));
                    }

                    ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                }
            }

            return View(newUser);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(AllUsers)); 
            }

            try
            {
                var user = await _accountRepository.GetUserByIdAsync(id);
                if (user is null)
                {
                    return RedirectToAction(nameof(AllUsers));
                }

                return View(user);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while getting the user.");
                return View(new ApplicationUser());
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> UserDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(AllUsers));
            }

            try
            {
                var user = await _accountRepository.GetUserByIdAsync(id);
                if (user is null)
                {
                    return RedirectToAction(nameof(AllUsers));
                }

                return View(user);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while getting the user.");
                return View(new ApplicationUser());
            }
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "User ID is null or empty." });
            }

            try
            {
                var result = await _accountRepository.DeleteUserAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "User successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = "User not found." });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while deleting the user." });
            }
        }
    }
}
