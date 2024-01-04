namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    /// <summary>
    /// Interface for retrieving system-wide statistics.
    /// </summary>
    public interface ISystemStatisticsRepository
    {
        /// <summary>
        /// Gets the total count of users in the system.
        /// </summary>
        /// <returns>The total number of users.</returns>
        Task<int> GetUserCountAsync();

        /// <summary>
        /// Gets the count of users who have been assigned the 'Customer' role.
        /// </summary>
        /// <returns>The number of users in the 'Customer' role.</returns>
        Task<int> GetCustomerRoleUserCountAsync();

        /// <summary>
        /// Gets the count of users who have been assigned the 'Delivery' role.
        /// </summary>
        /// <returns>The number of users in the 'Delivery' role.</returns>
        Task<int> GetDeliveryRoleUserCountAsync();

        /// <summary>
        /// Gets the count of users who have been assigned the 'Warehouse' role.
        /// </summary>
        /// <returns>The number of users in the 'Warehouse' role.</returns>
        Task<int> GetWarehouseRoleUserCountAsync();
    }
}
