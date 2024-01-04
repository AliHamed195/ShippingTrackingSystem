using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Enum;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class SystemStatisticsRepository : ISystemStatisticsRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SystemStatisticsRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<int> GetUserCountAsync()
        {
            try
            {
                return await _userManager.Users.CountAsync();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <inheritdoc />
        public async Task<int> GetCustomerRoleUserCountAsync()
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(UserRole.Customer.ToString());
                return usersInRole.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <inheritdoc />
        public async Task<int> GetDeliveryRoleUserCountAsync()
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(UserRole.Delivery.ToString());
                return usersInRole.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <inheritdoc />
        public async Task<int> GetWarehouseRoleUserCountAsync()
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(UserRole.Warehouse.ToString());
                return usersInRole.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

