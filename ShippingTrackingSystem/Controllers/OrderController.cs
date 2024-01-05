using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.ViewModels;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;

        public OrderController(IOrderRepository orderRepository, ICategoryRepository categoryRepository)
        {
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
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
                        AvailableQuantity = p.StockQuantity
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
    }

}

