﻿using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    /// <summary>
    /// Interface for managing categories, including CRUD operations.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Creates a new category in the system.
        /// </summary>
        /// <param name="category">The category to create.</param>
        /// <returns>A tuple containing a success flag, error message if any, and the created category.</returns>
        Task<(bool Succeeded, string ErrorMessage, Category Category)> CreateCategoryAsync(Category category);

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A tuple containing a success flag, error message if any, and a list of categories.</returns>
        Task<(bool Succeeded, string ErrorMessage, IEnumerable<Category> Categories)> GetAllCategoriesAsync();

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The unique identifier for the category.</param>
        /// <returns>A tuple containing a success flag, error message if any, and the category.</returns>
        Task<(bool Succeeded, string ErrorMessage, Category Category)> GetCategoryByIdAsync(int categoryId);

        /// <summary>
        /// Updates a given category.
        /// </summary>
        /// <param name="category">The category with updated information.</param>
        /// <returns>A tuple indicating success or failure, along with an error message if applicable.</returns>
        Task<(bool Succeeded, string ErrorMessage)> UpdateCategoryAsync(Category category);

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The unique identifier for the category to delete.</param>
        /// <returns>A tuple indicating success or failure, along with an error message if applicable.</returns>
        Task<(bool Succeeded, string ErrorMessage)> DeleteCategoryAsync(int categoryId);
    }
}
