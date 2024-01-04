using Microsoft.AspNetCore.Identity;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface IAccountRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> CreateUserAsync(ApplicationUser user, string password);
        Task<(bool Succeeded, string ErrorMessage)> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<IdentityResult?> RegisterUserAsync(ApplicationUser user, string password);
        Task<SignInResult> LoginUserAsync(string username, string password, bool rememberMe);
        Task<bool> LogoutUserAsync();
        Task<(bool Succeeded, string ErrorMessage)> AssignRoleAsync(ApplicationUser user, string roleName);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<string?> GetUserRoleAsync(ApplicationUser user);

    }
}
