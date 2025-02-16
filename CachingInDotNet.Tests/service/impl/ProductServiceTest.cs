//<copyright file="ProductServiceTest" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingInDotNet.exception;
using CachingInDotNet.models;
using CachingInDotNet.repository;
using CachingInDotNet.service.impl;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace CachingInDotNet.Tests.service.impl;


public class ProductServiceTest
{
    private readonly ProductService _productService;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IConnectionMultiplexer>  _redisConnectionMock;
    private readonly Mock<IDatabase> _redisDatabaseMock;
    private readonly List<Product> _products;

    
    //Setup method to initialize the mock object and the service object
    public ProductServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _redisConnectionMock = new Mock<IConnectionMultiplexer>();
        _redisDatabaseMock = new Mock<IDatabase>();
        
        // Mock Redis behavior
        _redisConnectionMock.Setup(redis => redis.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_redisDatabaseMock.Object);
        _productService = new ProductService(_productRepositoryMock.Object, _redisConnectionMock.Object);
        _products = new List<Product>();

        var product1 = new Product();
        
        product1.productId = new Guid("f5b1f1b1-0b1b-4b1b-8b1b-1b1b1b1b1b1b");
        product1.productName = "Product 1";
        product1.productDescription = "Product 1 Description";
        product1.productPrice = 100;
        product1.productQuantity = 10;
        product1.productCategory = "Category 1";
        product1.productCreatedDate = DateTime.UtcNow;
        product1.ExpiryDateTime = DateTime.UtcNow.AddDays(10);
        _products.Add(product1);
            
        var product2 = new Product();
        product2.productId =new Guid("2a2a745b-0e3d-4e50-8b65-bfa4dc800893");
        product2.productName = "Product 2";
        product2.productDescription = "Product 2 Description";
        product2.productPrice = 200;
        product2.productQuantity = 20;
        product2.productCategory = "Category 2";
        product2.productCreatedDate = DateTime.UtcNow;
        product2.ExpiryDateTime = DateTime.UtcNow.AddDays(20);
        _products.Add(product2);
    }
    
    /// <summary>
    /// Get all products success test
    /// </summary>
    [Fact]
    public async Task TestGetAllProductsSuccess()
    {
        //Arrange
        //Mock
        _productRepositoryMock
            .Setup(repo => 
                repo.GetAllAsync()).ReturnsAsync(_products);
        //Act
        /*
         * One thing to note here is that you cannot directly get the index
         * of IEnumerable<Product> result. You need to convert it to a list.
         * After then, you can get the index of the list.
         */
        var result = await _productService.GetProductsAsync();
        var enumerable = result.ToList();
        
        //Assert
        Assert.NotNull(result);
        Assert.NotNull(enumerable[0].productId.ToString());
        Assert.Equal("Product 1", enumerable[0].productName);
        Assert.Equal("Product 1 Description", enumerable[0].productDescription);
        Assert.Equal(100, enumerable[0].productPrice);
        Assert.Equal(10, enumerable[0].productQuantity);
        Assert.Equal("Category 1", enumerable[0].productCategory);
        Assert.Equal(DateTime.UtcNow.Date, enumerable[0].productCreatedDate.Date);
        Assert.Equal(DateTime.UtcNow.AddDays(10).Date, enumerable[0].ExpiryDateTime.Date);
        
        
    }
    
    /// <summary>
    /// Get product by id success test
    /// </summary>
    [Fact]
    public async Task TestGetProductByIdSuccess()
    {
        //Arrange
        var productId = new Guid("f5b1f1b1-0b1b-4b1b-8b1b-1b1b1b1b1b1b");
        //Mock
        _productRepositoryMock
            .Setup(repo => 
                repo.GetByIdAsync(productId)).ReturnsAsync(_products[0]);
        //Act
        var result = await _productService.GetProductByIdAsync(productId);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Product 1", result.productName);
        Assert.Equal("Product 1 Description", result.productDescription);
        Assert.Equal(100, result.productPrice);
        Assert.Equal(10, result.productQuantity);
        Assert.Equal("Category 1", result.productCategory);
        Assert.Equal(DateTime.UtcNow.Date, result.productCreatedDate.Date);
        Assert.Equal(DateTime.UtcNow.AddDays(10).Date, result.ExpiryDateTime.Date);
        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
    }

    /// <summary>
    /// Get product by id not found
    /// </summary>
    [Fact]
    public async Task TestGetProductByIdNotFound()
    {
        //Arrange
        var nonExistingProductId = new Guid("d76bb130-94da-4cfb-af1d-4b7f401c82a1");
        //Mock
        _productRepositoryMock
            .Setup(repo => 
                repo.GetByIdAsync(nonExistingProductId)).ReturnsAsync((Product) null);
        //Act and Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.GetProductByIdAsync(nonExistingProductId));
        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(nonExistingProductId), Times.Once);
    }

    /// <summary>
    /// Add product success test
    /// </summary>
    [Fact]
    public async Task TestAddProductSuccess()
    {
        //Arrange
        var newProduct = new Product();
        newProduct.productId = new Guid("d76bb130-94da-4cfb-af1d-4b7f401c82a1");
        newProduct.productName = "Product 3";
        newProduct.productDescription = "Product 3 Description";
        newProduct.productPrice = 300;
        newProduct.productQuantity = 30;
        newProduct.productCategory = "Category 3";
        newProduct.productCreatedDate = DateTime.UtcNow.AddDays(-1);
        newProduct.ExpiryDateTime = DateTime.UtcNow.AddDays(30);
        
        //Mock
        _productRepositoryMock
            .Setup(repo => 
                repo.CreateAsync(newProduct)).ReturnsAsync(newProduct);
        //Act
        var result = await _productService.AddProductAsync(newProduct);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Product 3", result.productName);
        Assert.Equal("Product 3 Description", result.productDescription);
        Assert.Equal(300, result.productPrice);
        Assert.Equal(30, result.productQuantity);
        Assert.Equal("Category 3", result.productCategory);
        Assert.Equal(DateTime.UtcNow.AddDays(-1).Date, result.productCreatedDate.Date);
        Assert.Equal(DateTime.UtcNow.AddDays(30).Date, result.ExpiryDateTime.Date);
        _productRepositoryMock.Verify(repo => repo.CreateAsync(newProduct), Times.Once);
    }

    /// <summary>
    /// Update product success test
    /// </summary>
    [Fact]
    public async Task TestUpdateProductSuccess()
    {
        //Arrange
        var productId = new Guid("f5b1f1b1-0b1b-4b1b-8b1b-1b1b1b1b1b1b");
        var updatedProduct = new Product();
        updatedProduct.productId = productId;
        updatedProduct.productName = "Product 1 Updated";
        updatedProduct.productDescription = "Product 1 Description Updated";
        updatedProduct.productPrice = 150;
        updatedProduct.productQuantity = 15;
        updatedProduct.productCategory = "Category 1 Updated";
        updatedProduct.productCreatedDate = DateTime.UtcNow;
        updatedProduct.ExpiryDateTime = DateTime.UtcNow.AddDays(15);
        
        //Mock
        //first call find and then update
        _productRepositoryMock
            .Setup(repo => 
                repo.GetByIdAsync(productId)).ReturnsAsync(_products[0]);
        
        //NOTE-->use It.IsAny<Product>() to match the updated product
        _productRepositoryMock
            .Setup(repo => 
                repo.UpdateAsync(productId, It.IsAny<Product>())).ReturnsAsync(updatedProduct);
        
        //Act
        var result = await _productService.UpdateProductAsync(productId, updatedProduct);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Product 1 Updated", result.productName);
        Assert.Equal("Product 1 Description Updated", result.productDescription);
        Assert.Equal(150, result.productPrice);
        Assert.Equal(15, result.productQuantity);
        Assert.Equal("Category 1 Updated", result.productCategory);
        Assert.Equal(DateTime.UtcNow.Date, result.productCreatedDate.Date);
        Assert.Equal(DateTime.UtcNow.AddDays(15).Date, result.ExpiryDateTime.Date);
        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
    }

    /// <summary>
    /// Delete product success test
    /// </summary>
    [Fact]
    public async Task TestDeleteProductSuccess()
    {
        //Arrange
        var productId = new Guid("f5b1f1b1-0b1b-4b1b-8b1b-1b1b1b1b1b1b");
        //Mock
        _productRepositoryMock
            .Setup(repo => 
                repo.GetByIdAsync(productId)).ReturnsAsync(_products[0]);
        //Act
        await _productService.DeleteProductAsync(productId);
        //Assert
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
    }

    /// <summary>
    /// Clear all cache success test
    /// </summary>
    [Fact]
    public async Task TestClearAllCacheSuccess()
    {
        //Arrange
        //Mock
        _productRepositoryMock
            .Setup(repo =>
                repo.GetAllAsync()).ReturnsAsync(_products);
        //Act
        await _productService.ClearAllCacheAsync();

        //Assert
        // Verify that the "products" cache key is deleted
        _redisDatabaseMock.Verify(redis => redis.KeyDeleteAsync("products", CommandFlags.None), Times.Once);

        // Verify that the individual product cache keys are deleted
        foreach (var product in _products)
        {
            var cacheKey = $"product_{product.productId}";
            _redisDatabaseMock.Verify(redis => redis.KeyDeleteAsync(cacheKey, CommandFlags.None), Times.Once);
        }

    }
}