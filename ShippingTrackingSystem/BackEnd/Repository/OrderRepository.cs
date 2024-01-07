using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Enum;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.Models.Context;
using ShippingTrackingSystem.ViewModels;
using System.Security.Claims;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly string _editorUserId;

        public OrderRepository(MyDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _editorUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _userManager = userManager;
            _productRepository = productRepository;
        }

        public async Task<(bool Succeeded, string ErrorMessage, Order Order)> CreateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await SaveOrderHistory(order.Id, order.Status, "Order created.");
                return (true, null, order);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage, IEnumerable<Order> Orders)> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                return (true, null, orders);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<Order>());
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage, Order Order)> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                return (order != null, order == null ? "Order not found." : null, order);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage)> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                await SaveOrderHistory(order.Id, order.Status, "Order updated.");
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage)> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();

                    await SaveOrderHistory(order.Id, order.Status, "Order deleted.");
                    return (true, null);
                }
                return (false, "Order not found.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<OrderHistory>> GetOrderHistoryAsync(int orderId)
        {
            try
            {
                return await _context.OrderHistory.Where(oh => oh.OrderId == orderId).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<OrderHistory>();
            }
        }

        private async Task SaveOrderHistory(int orderId, OrderStatus status, string notes)
        {
            var orderHistory = new OrderHistory
            {
                OrderId = orderId,
                Status = status,
                StatusChangedOn = DateTime.Now,
                EditorUserId = _editorUserId,
                Notes = notes
            };

            _context.OrderHistory.Add(orderHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Succeeded, string ErrorMessage, IEnumerable<Order> Orders)> GetAllOrdersByUserIdAsync(string userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(order => order.UserId == userId && !order.IsDeleted)
                    .ToListAsync();

                return (true, null, orders);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<IEnumerable<UserOrderViewModel>> GetUserOrdersAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId && !o.IsDeleted)
                    .Select(o => new UserOrderViewModel
                    {
                        OrderId = o.Id,
                        OrderDate = o.CreatedOn,
                        Status = o.Status.ToString(),
                        TotalAmount = (decimal)o.TotalAmount,
                        TrackingNumber = o.TrackingNumber,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        CustomerName = user.UserName,
                        CustomerEmail = user.Email,
                        OrderDetails = o.OrdersDetails.Select(od => new OrderDetailViewModel
                        {
                            ProductId = od.ProductId,
                            ProductName = od.Product.Name,
                            Quantity = od.Quantity,
                            UnitPrice = (decimal)od.UnitPrice
                        })
                    }).ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<UserOrderViewModel>();
            }
        }

        public async Task<IEnumerable<UserOrderViewModel>> GetOrdersByUserRoleAsync(string userId, UserRole userRole)
        {
            try
            {
                var ordersQuery = from o in _context.Orders
                                  join u in _context.Users on o.UserId equals u.Id
                                  where !o.IsDeleted
                                  select new { o, u }; 

                if (userRole == UserRole.Warehouse)
                {
                    ordersQuery = ordersQuery.Where(x =>
                        x.o.Status == OrderStatus.InWarehouse ||
                        x.o.Status == OrderStatus.Preparing ||
                        x.o.Status == OrderStatus.Pending);
                }
                else if (userRole == UserRole.Delivery)
                {
                    ordersQuery = ordersQuery.Where(x => x.o.Status == OrderStatus.Dispatched);
                }

                var orders = await ordersQuery.Select(x => new UserOrderViewModel
                {
                    OrderId = x.o.Id,
                    OrderDate = x.o.CreatedOn,
                    Status = x.o.Status.ToString(),
                    TotalAmount = (decimal)x.o.TotalAmount,
                    TrackingNumber = x.o.TrackingNumber,
                    EstimatedDeliveryDate = x.o.EstimatedDeliveryDate,
                    CustomerName = x.u.UserName,
                    CustomerEmail = x.u.Email,
                    OrderDetails = x.o.OrdersDetails.Select(od => new OrderDetailViewModel
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.Name,
                        Quantity = od.Quantity,
                        UnitPrice = (decimal)od.UnitPrice
                    }).ToList()
                }).ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<UserOrderViewModel>();
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage)> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.Include(o => o.OrdersDetails).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return (false, "The order not found");
            }

            if (newStatus == OrderStatus.Canceled && order.Status != OrderStatus.Canceled)
            {
                var productQuantities = order.OrdersDetails.ToDictionary(od => od.ProductId, od => od.Quantity);
                var revertSuccess = await _productRepository.RevertProductQuantitiesAsync(productQuantities);
                if (!revertSuccess)
                {
                    return (false, "can not Revert the items.");
                }
            }

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}
