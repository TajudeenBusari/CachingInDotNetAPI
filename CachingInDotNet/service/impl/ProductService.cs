using CachingInDotNet.exception;
using CachingInDotNet.models;
using CachingInDotNet.repository;

namespace CachingInDotNet.service.impl;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    
    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var foundProduct = await _productRepository.GetByIdAsync(productId)
            ?? throw new ProductNotFoundException(productId);
        return foundProduct;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }

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
        return await _productRepository.UpdateAsync(productId, existingProduct);
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId)
                              ?? throw new ProductNotFoundException(productId);
        await _productRepository.DeleteAsync(existingProduct.productId);
    }
}