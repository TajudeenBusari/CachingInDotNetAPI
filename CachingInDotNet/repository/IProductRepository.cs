//<copyright file="IProductRepository" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using CachingInDotNet.models;

namespace CachingInDotNet.repository;
/// <summary>
/// Product Repository interface which contains all the methods
/// </summary>
public interface IProductRepository
{
    Task <IEnumerable<Product>>GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(Guid id, Product product);
    Task DeleteAsync(Guid id);
    
}