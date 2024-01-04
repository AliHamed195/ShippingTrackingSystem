namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface ISystemStatisticsRepository
    {
        Task<int> GetUserCountAsync();
        Task<int> GetCustomerRoleUserCountAsync();
        Task<int> GetDeliveryRoleUserCountAsync();
        Task<int> GetWarehouseRoleUserCountAsync();
    }
}
