﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.BackEnd.Repository;
using ShippingTrackingSystem.Enum;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.ViewModels;
using System.Security.Claims;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderRepository orderRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _userManager = userManager;
        }

        [HttpGet("BuyProducts/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BuyProducts(int id)
        {
            var (success, errorMessage, products) = await _categoryRepository.GetAvailableProductsByCategoryIdAsync(categoryId: id);
            if (success)
            {
                var (categorySucceeded, categoryErrorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(categoryId: id);
                var model = new BuyProductsViewModel
                {
                    CategoryId = id,
                    CategoryName = category.Name,
                    Products = products.Select(p => new ProductOrderViewModel
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Price = p.Price,
                        AvailableQuantity = p.StockQuantity,
                        ImagePath = p.ImagePath
                    }).ToList()
                };
                return View(model);
            }

            var errorModel = new BuyProductsViewModel()
            {
                CategoryId = -1,
                Products = new List<ProductOrderViewModel>()
            };
            return View(errorModel);
        }

        [HttpPost("BuyProducts/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BuyProducts(int id, BuyProductsViewModel model)
        {
            bool isValid = model.Products.Any(p => p.PurchaseQuantity > 0);
            string error = null;
            if (isValid)
            {
                var order = new Order
                {
                    CreatedOn = DateTime.Now,
                    Status = OrderStatus.Pending,
                    UserId = _userId,
                };

                foreach (var item in model.Products)
                {
                    if (item.PurchaseQuantity > 0)
                    {
                        var (Succeeded, ErrorMessage, product) = await _productRepository.GetProductByIdAsync(item.ProductId);
                        if (product != null && product.StockQuantity >= item.PurchaseQuantity)
                        {
                            order.TotalAmount += item.PurchaseQuantity * product.Price;
                            order.OrdersDetails.Add(new OrderDetail
                            {
                                ProductId = item.ProductId,
                                Quantity = item.PurchaseQuantity,
                                UnitPrice = product.Price,
                            });

                            product.StockQuantity -= item.PurchaseQuantity;
                            await _productRepository.UpdateProductAsync(product);
                        }
                    }
                }
                var createOrderResult = await _orderRepository.CreateOrderAsync(order);
                if (createOrderResult.Succeeded)
                {
                    return RedirectToAction(controllerName: "", actionName: "");
                }
                error = createOrderResult.ErrorMessage;
            }
            else
            {
                error = "you should select at least one item";
            }

            ViewBag.error =  error;
            var (success, errorMessage, products) = await _categoryRepository.GetAvailableProductsByCategoryIdAsync(categoryId: id);
            var (categorySucceeded, categoryErrorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(categoryId: id);
            model = new BuyProductsViewModel
            {
                CategoryId = id,
                CategoryName = category.Name,
                Products = products.Select(p => new ProductOrderViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    AvailableQuantity = p.StockQuantity,
                    ImagePath = p.ImagePath
                }).ToList()
            };
            return View(model);
        }

        [HttpGet("UserOrders")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            return View(orders);
        }

        // GET: Order/UserOrders
        [HttpGet("AllOrders")]
        [Authorize(Roles = "Delivery, Warehouse")]
        public async Task<IActionResult> GetAllUsersOrders()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            UserRole userRole = DetermineUserRole(userRoles);

            var orders = await _orderRepository.GetOrdersByUserRoleAsync(userId, userRole);
            return View(orders);
        }

        [HttpPost("UpdateStatus")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus newStatus)
        {
            var (Succeeded, ErrorMessage) = await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);

            if (Succeeded)
            {
                return Json(new { success = true, message = "Order status updated successfully." });
            }
            else
            {
                return Json(new { success = false, message = ErrorMessage });
            }
        }

        private UserRole DetermineUserRole(IList<string> userRoles)
        {
            if (userRoles.Contains("Warehouse"))
            {
                return UserRole.Warehouse;
            }
            else if (userRoles.Contains("Delivery"))
            {
                return UserRole.Delivery;
            }
            return UserRole.Admin;
        }

    }

}

