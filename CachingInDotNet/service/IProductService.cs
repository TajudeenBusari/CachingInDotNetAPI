using CachingInDotNet.models;

namespace CachingInDotNet.service;

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Guid productId, Product product);
    Task DeleteProductAsync(Guid productId);
    
    Task ClearAllCacheAsync();
    
}