using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(bool Succeeded, string ErrorMessage, Category Category)> CreateCategoryAsync(Category category);
        Task<(bool Succeeded, string ErrorMessage, IEnumerable<Category> Categories)> GetAllCategoriesAsync();
        Task<(bool Succeeded, string ErrorMessage, Category Category)> GetCategoryByIdAsync(int categoryId);
        Task<(bool Succeeded, string ErrorMessage)> UpdateCategoryAsync(Category category);
        Task<(bool Succeeded, string ErrorMessage)> DeleteCategoryAsync(int categoryId);
    }
}
