using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.BackEnd.Interfaces
{
    public interface IProductRepository
    {
        Task<(bool Succeeded, string ErrorMessage, Product Product)> CreateProductAsync(Product product);
        Task<(bool Succeeded, string ErrorMessage, IEnumerable<Product> Products)> GetAllProductsAsync();
        Task<(bool Succeeded, string ErrorMessage, Product Product)> GetProductByIdAsync(int productId);
        Task<(bool Succeeded, string ErrorMessage)> UpdateProductAsync(Product product);
        Task<(bool Succeeded, string ErrorMessage)> DeleteProductAsync(int productId);
        Task<bool> RevertProductQuantitiesAsync(Dictionary<int, int> productQuantities);

    }
}
