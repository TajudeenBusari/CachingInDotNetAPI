//<copyright file="IProductService" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using CachingInDotNet.models;

namespace CachingInDotNet.service;

/// <summary>
/// Product service interface
/// </summary>
public interface IProductService
{
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Guid productId, Product product);
    Task DeleteProductAsync(Guid productId);
    
    Task ClearAllCacheAsync();
    
}