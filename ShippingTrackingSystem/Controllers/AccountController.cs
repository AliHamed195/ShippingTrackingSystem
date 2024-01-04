﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

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
                var result = await _accountRepository.LoginUserAsync(email, password, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View();
            }
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string roleName = UserRole.Customer.ToString();
                    var result = await _accountRepository.RegisterUserAsync(user, password);
                    if (result.Succeeded)
                    {
                        var roleAssignResult = await _accountRepository.AssignRoleAsync(user, roleName);
                        if (roleAssignResult)
                        {
                            return RedirectToAction("UserList");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to assign the role.");
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _accountRepository.LogoutUserAsync();
            }

            return RedirectToAction(controllerName: "Account", actionName: "Login");
        }
    }
}
