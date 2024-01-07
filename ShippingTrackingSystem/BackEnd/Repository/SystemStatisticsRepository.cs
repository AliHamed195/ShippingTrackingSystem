using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Enum;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.Models.Context;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class SystemStatisticsRepository : ISystemStatisticsRepository
    {
        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SystemStatisticsRepository(UserManager<ApplicationUser> userManager, MyDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

        public async Task<Dictionary<string, int>> GetOrderStatusCountsAsync()
        {
            return await _context.Orders
                                 .AsNoTracking()
                                 .GroupBy(order => order.Status)
                                 .Select(group => new { Status = group.Key.ToString(), Count = group.Count() })
                                 .ToDictionaryAsync(k => k.Status, v => v.Count);
        }

        public async Task<int> GetAvailableProductsCountAsync()
        {
            try
            {
                return await _context.Products.CountAsync(p => !p.IsDeleted && p.StockQuantity > 0);
            }
            catch (Exception) { return 0; }
        }

        public async Task<int> GetOutOfStockProductsCountAsync()
        {
            try
            {
                return await _context.Products.CountAsync(p => !p.IsDeleted && p.StockQuantity == 0);
            }
            catch (Exception) { return 0; }
        }
    }
}

