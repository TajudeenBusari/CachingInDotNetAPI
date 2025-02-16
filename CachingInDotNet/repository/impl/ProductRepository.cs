//<copyright file="ProductRepository" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using CachingInDotNet.Data;
using CachingInDotNet.models;
using Microsoft.EntityFrameworkCore;

namespace CachingInDotNet.repository.impl;

/// <summary>
/// Product Repository class which implements the IProductRepository interface
/// </summary>
public class ProductRepository: IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get all products from the database
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    /// <summary>
    /// Get a product by its id from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    /// <summary>
    /// Create a new product in the database
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Update a product in the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Product?> UpdateAsync(Guid id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return null;
        }
        existingProduct.productName = product.productName;
        existingProduct.productDescription = product.productDescription;
        existingProduct.productCategory = product.productCategory;
        existingProduct.productPrice = product.productPrice;
        existingProduct.productQuantity = product.productQuantity;
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    /// <summary>
    /// Delete a product from the database
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return;
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}