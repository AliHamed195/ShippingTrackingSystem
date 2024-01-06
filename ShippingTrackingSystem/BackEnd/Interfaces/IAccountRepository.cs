using Microsoft.AspNetCore.Identity;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    /// <summary>
    /// Interface for account-related data operations.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier for the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<ApplicationUser?> GetUserByIdAsync(string userId);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        /// <summary>
        /// Creates a new user with the specified password.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The created user if successful; otherwise, null.</returns>
        Task<ApplicationUser?> CreateUserAsync(ApplicationUser user, string password);

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>A tuple indicating success or failure, along with an error message if applicable.</returns>
        Task<(bool Succeeded, string ErrorMessage)> UpdateUserAsync(ApplicationUser user);

        /// <summary>
        /// Deletes the user with the specified unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier for the user.</param>
        /// <returns>True if the user was successfully deleted; otherwise, false.</returns>
        Task<bool> DeleteUserAsync(string userId);

        /// <summary>
        /// Registers a new user with the specified password.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>An IdentityResult indicating the outcome of the operation.</returns>
        Task<IdentityResult?> RegisterUserAsync(ApplicationUser user, string password);

        /// <summary>
        /// Logs in a user with the specified username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="rememberMe">Whether to remember the user for future logins.</param>
        /// <returns>A SignInResult indicating the outcome of the login attempt.</returns>
        Task<(bool Succeeded, string ErrorMessage, SignInResult Result)> LoginUserAsync(string username, string password, bool rememberMe);

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>True if the user was successfully logged out; otherwise, false.</returns>
        Task<bool> LogoutUserAsync();

        /// <summary>
        /// Assigns the specified role to the user.
        /// </summary>
        /// <param name="user">The user to assign the role to.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>A tuple indicating success or failure, along with an error message if applicable.</returns>
        Task<(bool Succeeded, string ErrorMessage)> AssignRoleAsync(ApplicationUser user, string roleName);

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        /// <returns>A list of all roles.</returns>
        Task<List<IdentityRole>> GetRolesAsync();

        /// <summary>
        /// Retrieves the role of the specified user.
        /// </summary>
        /// <param name="user">The user whose role is to be retrieved.</param>
        /// <returns>The role of the user if found; otherwise, null.</returns>
        Task<string?> GetUserRoleAsync(ApplicationUser user);
    }
}
