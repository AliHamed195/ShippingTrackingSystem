using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.Models.Context;
using ShippingTrackingSystem.Services.Interfaces;

namespace ShippingTrackingSystem.BackEnd.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;
        private readonly IFileService _fileService;

        public ProductRepository(MyDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<(bool Succeeded, string ErrorMessage, Product Product)> CreateProductAsync(Product product)
        {
            try
            {
                var temp = _context.Products.Where(p => p.Name == product.Name && p.IsDeleted == false && p.CategoryId == product.CategoryId).FirstOrDefault();
                if (temp is not null)
                {
                    return (false, "The name is exist.", null);
                }

                if (product.ImageFile != null)
                {
                    product.ImagePath = await _fileService.AddFileAsync(product.ImageFile);
                }
                else
                {
                    return (false, "Image is required.", null);
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return (true, null, product);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage, IEnumerable<Product> Products)> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();
                return (true, null, products);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<Product>());
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage, Product Product)> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    return (true, null, product);
                }
                return (false, "Product not found.", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage)> UpdateProductAsync(Product product)
        {
            try
            {
                var temp = _context.Products.Where(p =>
                p.Name == product.Name && p.IsDeleted == false &&
                p.CategoryId == product.CategoryId && p.Id != product.Id
                ).FirstOrDefault();

                if (temp is not null)
                {
                    return (false, "The name is exist.");
                }

                if (product.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        _fileService.DeleteFileAsync(product.ImagePath);
                    }
                    product.ImagePath = await _fileService.AddFileAsync(product.ImageFile);
                }
                else if (product.ImagePath is null)
                {
                    return (false, "Image is required.");
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Succeeded, string ErrorMessage)> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        _fileService.DeleteFileAsync(product.ImagePath);
                    }

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return (true, null);
                }
                return (false, "Product not found.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
