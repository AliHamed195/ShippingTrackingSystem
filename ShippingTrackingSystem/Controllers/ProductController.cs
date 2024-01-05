﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.BackEnd.Repository;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Product/All
        [HttpGet("All")]
        public async Task<IActionResult> AllProducts()
        {
            var (success, errorMessage, products) = await _productRepository.GetAllProductsAsync();
            if (success)
            {
                return View(products);
            }

            ModelState.AddModelError("", errorMessage);
            return View(Enumerable.Empty<Product>());
        }

        // GET: Product/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var (success, errorMessage, categories) = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var (success, errorMessage, createdProduct) = await _productRepository.CreateProductAsync(product);
                if (success)
                {
                    return RedirectToAction(nameof(AllProducts));
                }

                ModelState.AddModelError("", errorMessage);
            }

            var (categorySuccess, categoryErrorMessage, categories) = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        // GET: Product/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var (success, errorMessage, product) = await _productRepository.GetProductByIdAsync(id);
            if (success && product != null)
            {
                return View(product);
            }

            ModelState.AddModelError("", errorMessage);
            return View(new Product());
        }

        // POST: Product/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                var (success, errorMessage) = await _productRepository.UpdateProductAsync(product);
                if (success)
                {
                    return RedirectToAction(nameof(AllProducts));
                }

                ModelState.AddModelError("", errorMessage);
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (success, errorMessage) = await _productRepository.DeleteProductAsync(id);
                if (success)
                {
                    return Json(new { success = true, message = "Product successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while deleting the Product." });
            }
        }

    }
}
