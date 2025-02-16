//<copyright file="ProductService" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using System.Collections;
using System.Text.Json;
using CachingInDotNet.exception;
using CachingInDotNet.models;
using CachingInDotNet.repository;
using StackExchange.Redis;

namespace CachingInDotNet.service.impl;

/// <summary>
/// Product service implementation
/// </summary>
public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    
    private readonly IDatabase _cacheDb;  // Redis Cache Database

    public ProductService(IProductRepository productRepository, IConnectionMultiplexer redis)
    {
        _productRepository = productRepository;
        _cacheDb = redis.GetDatabase();
    }
    
    /// <summary>
    /// Get product by Id
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException"></exception>
    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var cacheKey = $"product_{productId}";
        
        //check if the product is in the cache
        var cachedProduct = await _cacheDb.StringGetAsync(cacheKey);
        if (!cachedProduct.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<Product>(cachedProduct!);
        }
        
        var foundProduct = await _productRepository.GetByIdAsync(productId)
            ?? throw new ProductNotFoundException(productId);
        
        //store the product in the cache
        var cacheOptions = new TimeSpan(0, 10, 0); //10 minutes
        await _cacheDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(foundProduct), cacheOptions);
        return foundProduct;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        //check if the products are in the cache
        var cacheKey = "products";
        var cachedProducts = await _cacheDb.StringGetAsync(cacheKey);
        if (!cachedProducts.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<IEnumerable<Product>>(cachedProducts!)!;
        }
        var productsFromDb = await _productRepository.GetAllAsync();
        
        //store the products in the cache with 5 minutes expiration
        await _cacheDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(productsFromDb), TimeSpan.FromMinutes(10));
        return productsFromDb;
    }

    /// <summary>
    /// Add a new product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Product> AddProductAsync(Product product)
    {
        //cache new product
        var cacheKey = $"product_{product.productId}";
        var cacheOptions = new TimeSpan(0, 10, 0); //10 minutes
        await _cacheDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(product), cacheOptions);
        
        //clear cache for all products
        await _cacheDb.KeyDeleteAsync("products");
        return await _productRepository.CreateAsync(product);
    }

    /// <summary>
    /// Update a product
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException"></exception>
    public async Task<Product?> UpdateProductAsync(Guid productId, Product product)
    {

        var existingProduct = await _productRepository.GetByIdAsync(productId)
                              ?? throw new ProductNotFoundException(productId);
        
        //update the fields of the existing product except the productId, productCreatedDate and ExpiryDateTime
        existingProduct.productName = product.productName;
        existingProduct.productDescription = product.productDescription;
        existingProduct.productPrice = product.productPrice;
        existingProduct.productQuantity = product.productQuantity;
        existingProduct.productCategory = product.productCategory;
        
        //update the product in the database
        var updatedProduct = await _productRepository.UpdateAsync(productId, existingProduct);
        
        //update the product in the cache
        var cacheKey = $"product_{productId}";
        var cacheOptions = new TimeSpan(0, 10, 0); //10 minutes
        
        //clear the cached product
        await _cacheDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(updatedProduct), cacheOptions);
        return updatedProduct;
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId)
                              ?? throw new ProductNotFoundException(productId);
        await _productRepository.DeleteAsync(existingProduct.productId);
        
        //remove the product from the cache
        var cacheKey = $"product_{productId}";
        await _cacheDb.KeyDeleteAsync(cacheKey);
    }

    /// <summary>
    /// clear all cache
    /// </summary>
    public async Task ClearAllCacheAsync()
    {
        var cacheKeys = new List<string>
        {
            "products"
        };
        foreach (var cacheKey in cacheKeys)
        {
            await _cacheDb.KeyDeleteAsync(cacheKey);
        }
        
        //clear all cached individual products
        var allProducts = await _productRepository.GetAllAsync();
        foreach (var product in allProducts)
        {
            var productCacheKey = $"product_{product.productId}";
            await _cacheDb.KeyDeleteAsync(productCacheKey);
        }
        
    }
}