﻿using Microsoft.AspNetCore.Identity;
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
        private readonly string _editorUserId;

        public OrderRepository(MyDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _editorUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _userManager = userManager;
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

        
    }
}
