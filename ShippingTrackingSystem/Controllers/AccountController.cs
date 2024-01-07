using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Enum;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: Account/Login
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Please write both email and password.");
                return View();
            }

            try
            {
                var(Succeeded, ErrorMessage, Result) = await _accountRepository.LoginUserAsync(username: email, password: password, rememberMe: false);
                if (Succeeded)
                {
                    return RedirectToAction(controllerName: "Home", actionName: "HomePage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ErrorMessage}");
                    return View();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View();
            }
        }

        // GET: Account/Register
        [HttpGet("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string roleName = UserRole.Customer.ToString();
                    var result = await _accountRepository.RegisterUserAsync(user: user, password: password);
                    if (result.Succeeded)
                    {
                        var (updateRoleSucceeded, updateRoleErrorMessage) = await _accountRepository.AssignRoleAsync(user: user, roleName: roleName);
                        if (updateRoleSucceeded)
                        {
                            return RedirectToAction(nameof(Login));
                        }
                        else
                        {
                            ModelState.AddModelError("", $"Failed to assign the role: {updateRoleErrorMessage}");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                }
            }

            return View(user);
        }

        // POST: Account/Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _accountRepository.LogoutUserAsync();
            }

            return RedirectToAction(controllerName: "Home", actionName: "HomePage");
        }
    }
}
