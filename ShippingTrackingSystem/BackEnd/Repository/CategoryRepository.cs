﻿using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.Models.Context;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;

        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage, Category Category)> CreateCategoryAsync(Category category)
        {
            try
            {
                var temp = _context.Categories.Where(c => c.Name == category.Name && !c.IsDeleted).FirstOrDefault(); 
                if (temp is not null)
                {
                    return (false, "The name is exist.", null);
                }
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return (true, null, category);
            }
            catch (Exception ex)
            {
                return (false, "There were an error while creating the new category", null);
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage, IEnumerable<Category> Categories)> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories.Where(c => !c.IsDeleted).Include(c => c.Products).ToListAsync();
                foreach (var category in categories)
                {
                    category.IsContainsProducts = category.Products?.Any() ?? false;
                }
                return (true, null, categories);
            }
            catch (Exception ex)
            {
                return (false, "There were an error while getting all the categories", new List<Category>());
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage, Category Category)> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    return (true, null, category);
                }
                return (false, "Category not found.", null);
            }
            catch (Exception ex)
            {
                return (false, "There were an error while getting the category", null);
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage)> UpdateCategoryAsync(Category category)
        {
            try
            {
                var temp = _context.Categories.Where(c => c.Name == category.Name && !c.IsDeleted && c.Id != category.Id).FirstOrDefault();
                if (temp is not null)
                {
                    return (false, "The name is exist.");
                }
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "There were an error while updating the category");
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage)> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    category.IsDeleted = true;
                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();
                    return (true, null);
                }
                return (false, "Category not found.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <inheritdoc />
        public async Task<(bool Succeeded, string ErrorMessage, IEnumerable<Product> Products)> GetProductsByCategoryIdAsync(int categoryId)
        {
            try
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == categoryId);
                if (!categoryExists)
                {
                    return (false, "Category not found.", Enumerable.Empty<Product>());
                }

                var products = await _context.Products
                    .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                    .ToListAsync();

                return (true, null, products);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, Enumerable.Empty<Product>());
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage, IEnumerable<Product> Products)> GetAvailableProductsByCategoryIdAsync(int categoryId)
        {
            try
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == categoryId && !c.IsDeleted);
                if (!categoryExists)
                {
                    return (false, "Category not found.", Enumerable.Empty<Product>());
                }

                var products = await _context.Products
                    .Where(p => p.CategoryId == categoryId && p.StockQuantity > 0 && !p.IsDeleted)
                    .ToListAsync();

                return (true, null, products);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, Enumerable.Empty<Product>());
            }
        }
    }
}
