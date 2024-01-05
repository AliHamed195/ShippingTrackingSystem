using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface IOrderRepository
    {
        Task<(bool Succeeded, string ErrorMessage, Order Order)> CreateOrderAsync(Order order);
        Task<(bool Succeeded, string ErrorMessage, IEnumerable<Order> Orders)> GetAllOrdersAsync();
        Task<(bool Succeeded, string ErrorMessage, Order Order)> GetOrderByIdAsync(int orderId);
        Task<(bool Succeeded, string ErrorMessage)> UpdateOrderAsync(Order order);
        Task<(bool Succeeded, string ErrorMessage)> DeleteOrderAsync(int orderId);
        Task<IEnumerable<OrderHistory>> GetOrderHistoryAsync(int orderId);
    }
}
