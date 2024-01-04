﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> CreateUser()
        {
            var roles = await _accountRepository.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(ApplicationUser user, string roleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string defaultPassword = _configuration["DefaultPassword"];
                    var result = await _accountRepository.RegisterUserAsync(user, defaultPassword);
                    if (result.Succeeded)
                    {
                        var (updateRoleSucceeded, updateRoleErrorMessage) = await _accountRepository.AssignRoleAsync(user, roleName);
                        if (updateRoleSucceeded)
                        {
                            return RedirectToAction(nameof(AllUsers));
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

            var roles = await _accountRepository.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View(user);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(AllUsers));
            }

            var user = await _accountRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(AllUsers));
            }

            var roles = await _accountRepository.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name", await _accountRepository.GetUserRoleAsync(user));

            return View(user);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, ApplicationUser user, string roleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = await _accountRepository.GetUserByIdAsync(id);
                    if (oldUser == null)
                    {
                        return RedirectToAction(nameof(AllUsers));
                    }

                    oldUser.UserName = user.UserName;
                    oldUser.Email = user.Email;
                    oldUser.Address = user.Address;

                    var (updateUserSucceeded, updateUserErrorMessage) = await _accountRepository.UpdateUserAsync(oldUser);
                    if (updateUserSucceeded)
                    {
                        var userOldRole = await _accountRepository.GetUserRoleAsync(user);
                        if (userOldRole != null && userOldRole != roleName)
                        {
                            var (updateRoleSucceeded, updateRoleErrorMessage) = await _accountRepository.AssignRoleAsync(oldUser, roleName);
                            if (updateRoleSucceeded)
                            {
                                return RedirectToAction(nameof(AllUsers));
                            }
                            else
                            {
                                ModelState.AddModelError("", $"Failed to update the user role: {updateRoleErrorMessage}");
                            }
                        }
                        else
                        {
                            return RedirectToAction(nameof(AllUsers));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Failed to update the user: {updateUserErrorMessage}");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while updating the user.");
                }
            }

            var roles = await _accountRepository.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, "Name", "Name", await _accountRepository.GetUserRoleAsync(user));

            return View(user);
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
