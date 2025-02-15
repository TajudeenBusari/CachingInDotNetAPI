using CachingInDotNet.models;

namespace CachingInDotNet.repository;

public interface IProductRepository
{
    Task <IEnumerable<Product>>GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(Guid id, Product product);
    Task DeleteAsync(Guid id);
    
}