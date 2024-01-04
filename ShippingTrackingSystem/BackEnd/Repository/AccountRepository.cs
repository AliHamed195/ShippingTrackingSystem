using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

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

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await Task.FromResult(_userManager.Users.ToList());
            }
            catch (Exception)
            {
                return Enumerable.Empty<ApplicationUser>();
            }
        }

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

        public async Task<SignInResult> LoginUserAsync(string username, string password, bool rememberMe)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

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

        public async Task<string?> GetUserRoleAsync(ApplicationUser user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
