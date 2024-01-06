using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;
using System.Security.Claims;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public async Task<ApplicationUser?> CreateUserAsync(ApplicationUser user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    return user;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    return result.Succeeded;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                string currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await Task.FromResult(_userManager.Users.Where(u => u.Id != currentUserId).ToList());
            }
            catch (Exception)
            {
                return Enumerable.Empty<ApplicationUser>();
            }
        }

        /// <inheritdoc />
        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage)> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return (true, null);
                }

                return (false, result.Errors.FirstOrDefault()?.Description ?? "Unknown error");
            }
            catch (Exception)
            {
                return (true, "An error happednd. Please try again later.");
            }
        }

        /// <inheritdoc />
        public async Task<IdentityResult?> RegisterUserAsync(ApplicationUser user, string password)
        {
            try
            {
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage, SignInResult Result)> LoginUserAsync(string username, string password, bool rememberMe)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return (true, string.Empty, result);
                }
                else if (result.IsLockedOut)
                {
                    return (false, "User account is locked out.", result);
                }
                else if (result.IsNotAllowed)
                {
                    return (false, "User is not allowed to log in.", result);
                }
                else if (result.RequiresTwoFactor)
                {
                    return (false, "Two-factor authentication is required.", result);
                }
                else
                {
                    return (false, "Invalid login attempt.", result);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Unexpected error: {ex.Message}", null);
            }
        }


        /// <inheritdoc />
        public async Task<bool> LogoutUserAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage)> AssignRoleAsync(ApplicationUser user, string roleName)
        {
            try
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    return (false, "Please select a valid role.");
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if(result.Succeeded)
                {
                    return (true, null);
                }

                return (false, result.Errors.FirstOrDefault()?.Description ?? "Unknown error");
            }
            catch(Exception)
            {
                return (true, "An error happednd. Please try again later.");
            }
        }

        /// <inheritdoc />
        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return roles;
            }
            catch (Exception)
            {
                return new List<IdentityRole>();
            }
        }

        /// <inheritdoc />
        public async Task<string?> GetUserRoleAsync(ApplicationUser user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
