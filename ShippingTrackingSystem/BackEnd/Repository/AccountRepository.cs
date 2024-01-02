using Microsoft.AspNetCore.Identity;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
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
    }
}
