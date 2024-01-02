using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface IAccountRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
    }
}
