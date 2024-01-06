﻿using Microsoft.AspNetCore.Mvc;
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

        public OrderController(IOrderRepository orderRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet("BuyProducts/{id}")]
        public async Task<IActionResult> BuyProducts(int id)
        {
            var (success, errorMessage, products) = await _categoryRepository.GetAvailableProductsByCategoryIdAsync(categoryId: id);
            if (success)
            {
                var(categorySucceeded, categoryErrorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(categoryId: id);
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
        public async Task<IActionResult> BuyProducts(int id,BuyProductsViewModel model)
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
                    var (Succeeded,  ErrorMessage,  product) = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product != null && product.StockQuantity >= item.PurchaseQuantity)
                    {
                        order.TotalAmount += item.PurchaseQuantity * product.Price;
                        order.OrdersDetails.Add(new OrderDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.PurchaseQuantity,
                        });

                        product.StockQuantity -= item.PurchaseQuantity;
                        await _productRepository.UpdateProductAsync(product);
                    }
                }
            }

            var createOrderResult = await _orderRepository.CreateOrderAsync(order);
            if (!createOrderResult.Succeeded)
            {
                ModelState.AddModelError("", createOrderResult.ErrorMessage);
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

            return RedirectToAction(controllerName: "", actionName: "");
        }


    }

}

