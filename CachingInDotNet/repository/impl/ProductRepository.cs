using CachingInDotNet.Data;
using CachingInDotNet.models;
using Microsoft.EntityFrameworkCore;

namespace CachingInDotNet.repository.impl;

public class ProductRepository: IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

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