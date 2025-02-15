
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CachingInDotNet.controller;
using CachingInDotNet.models;
using CachingInDotNet.models.dto;
using CachingInDotNet.repository;
using CachingInDotNet.service;
using CachingInDotNet.service.impl;
using CachingInDotNet.system;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CachingInDotNet.Tests.controller;

/**
 * This class tests the ProductController
 * I am using the Repository layer mocking technique to test the controller
 * It is a good practice to test the controller with the service layer mocked
 * and this will be tried as well later.
 */
public class ProductControllerTest
{
    private readonly ProductController _productController;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly ProductService _productService;
    private readonly List<Product> _products;
    private ITestOutputHelper _output;
    
    public ProductControllerTest(ITestOutputHelper output)
    {
        _productRepository = new Mock<IProductRepository>(); //initialize the mock
        _productService = new ProductService(_productRepository.Object);
        _productController = new ProductController(_productService);
        _products = new List<Product>();
        _output = output;
        
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
        product2.productId = new Guid("2a2a745b-0e3d-4e50-8b65-bfa4dc800893");
        product2.productName = "Product 2";
        product2.productDescription = "Product 2 Description";
        product2.productPrice = 200;
        product2.productQuantity = 20;
        product2.productCategory = "Category 2";
        product2.productCreatedDate = DateTime.UtcNow;
        product2.ExpiryDateTime = DateTime.UtcNow.AddDays(20);
        _products.Add(product2);
    }

    [Fact]
    public async Task TestGetAllProductsSuccess()
    {
        //Arrange
        
        //Mock
        _productRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(_products);
        
        //Act
        var result = await _productController.GetAllProducts();
        
        //Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result).Value;
        var actualResult = Assert.IsType<Result>(okResult);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(200, actualResult.Code);
        Assert.Equal("Find All Success", actualResult.Message);
        Assert.IsType<List<ProductDto>>(actualResult.Data);
        var actualResultData = actualResult.Data as List<ProductDto>;
        Assert.NotNull(actualResultData);
        Assert.Equal(_products[0].productName, actualResultData[0].productName);
        Assert.Equal(_products[1].productName, actualResultData[1].productName);
        Assert.Equal(_products[0].productDescription, actualResultData[0].productDescription);
        Assert.Equal(_products[1].productDescription, actualResultData[1].productDescription);
        Assert.Equal(_products[0].productPrice, actualResultData[0].productPrice);
        Assert.Equal(_products[1].productPrice, actualResultData[1].productPrice);
        Assert.Equal(_products[0].productQuantity, actualResultData[0].productQuantity);
        Assert.Equal(_products[1].productQuantity, actualResultData[1].productQuantity);
        Assert.Equal(_products[0].productCategory, actualResultData[0].productCategory);
        Assert.Equal(_products[1].productCategory, actualResultData[1].productCategory);
        Assert.Equal(_products[0].ExpiryDateTime.Date, actualResultData[0].ExpiryDateTime.Date);
        Assert.Equal(_products[1].ExpiryDateTime.Date, actualResultData[1].ExpiryDateTime.Date);
        //NOTE: productCreatedDate does not exist in the ProductDto
    }
    
    [Fact]
    public async Task TestGetProductByIdSuccess()
    {
        //Arrange
        var productId = _products[0].productId;
        
        //Mock
       _productRepository
            .Setup(repository => repository.GetByIdAsync(productId))
            .ReturnsAsync(_products[0]);
        
        //Act
        var result = await _productController.GetProductById(productId);
        
        //Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result).Value;
        var actualResult = Assert.IsType<Result>(okResult);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(200, actualResult.Code);
        Assert.Equal("Find One Success", actualResult.Message);
        Assert.IsType<ProductDto>(actualResult.Data);
        var actualResultData = actualResult.Data as ProductDto;
        Assert.NotNull(actualResultData);
        Assert.Equal(_products[0].productName, actualResultData.productName);
        Assert.Equal(_products[0].productDescription, actualResultData.productDescription);
        Assert.Equal(_products[0].productPrice, actualResultData.productPrice);
        Assert.Equal(_products[0].productQuantity, actualResultData.productQuantity);
        Assert.Equal(_products[0].productCategory, actualResultData.productCategory);
        Assert.Equal(_products[0].ExpiryDateTime.Date, actualResultData.ExpiryDateTime.Date);
        //NOTE: productCreatedDate does not exist in the ProductDto
    }
    
    [Fact]
    public async Task TestAddProductSuccess()
    {
        //Arrange
        var createProductDto = new CreateProductDto(
            "Product 3",
            "Product 3 Description",
            300,
            30,
            "Category 3",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(30)
        );
        
        //Mock
        _productRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(_products[0]);
        
        //Act
        var result = await _productController.AddProduct(createProductDto);
        
        //Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result).Value;
        var actualResult = Assert.IsType<Result>(okResult);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(200, actualResult.Code);
        Assert.Equal("Add One Success", actualResult.Message);
        Assert.IsType<ProductDto>(actualResult.Data);
        var actualResultData = actualResult.Data as ProductDto;
        Assert.NotNull(actualResultData);
        Assert.Equal(_products[0].productName, actualResultData.productName);
        Assert.Equal(_products[0].productDescription, actualResultData.productDescription);
        Assert.Equal(_products[0].productPrice, actualResultData.productPrice);
        Assert.Equal(_products[0].productQuantity, actualResultData.productQuantity);
        Assert.Equal(_products[0].productCategory, actualResultData.productCategory);
        Assert.Equal(_products[0].ExpiryDateTime.Date, actualResultData.ExpiryDateTime.Date);
        //NOTE: productCreatedDate does not exist in the ProductDto
    }
    
    [Fact]
    public async Task TestUpdateProductSuccess()
    {
        //Arrange
        
        //old product
        var productId = _products[0].productId;
        
        //update product
        var updateProductDto = new UpdateProductDto(
            "Product 1",
            "Product 1 Description update",
            300,
            30,
            "Category 1 update"
        );
        //update that will be returned by the service
        var updatedProduct = new Product();
        updatedProduct.productId = productId; //first product id in the list
        updatedProduct.productName = updateProductDto.productName;
        updatedProduct.productDescription = updateProductDto.productDescription;
        updatedProduct.productPrice = updateProductDto.productPrice;
        updatedProduct.productQuantity = updateProductDto.productQuantity;
        updatedProduct.productCategory = updateProductDto.productCategory;
        updatedProduct.productCreatedDate = _products[0].productCreatedDate; //cannot be updated
        updatedProduct.ExpiryDateTime = _products[0].ExpiryDateTime; //cannot be updated
        
        //Mock
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId))
            .ReturnsAsync(_products[0]);
        _productRepository.Setup(repo => repo.UpdateAsync(_products[0].productId, It.IsAny<Product>()))
            .ReturnsAsync(updatedProduct);
        
        //Act
        var result = await _productController.UpdateProduct(productId, updateProductDto);
        
        //Assert
        Assert.NotNull(result);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result).Value;
        var actualResult = Assert.IsType<Result>(okResult);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(200, actualResult.Code);
        Assert.Equal("Update One Success", actualResult.Message);
        Assert.IsType<ProductDto>(actualResult.Data);
        var actualResultData = actualResult.Data as ProductDto;
        _output.WriteLine(actualResultData?.ToString());
        Assert.NotNull(actualResultData);
        Assert.Equal(updateProductDto.productName, actualResultData.productName);
        Assert.Equal(updateProductDto.productDescription, actualResultData.productDescription);
        Assert.Equal(updateProductDto.productPrice, actualResultData.productPrice);
        Assert.Equal(updateProductDto.productQuantity, actualResultData.productQuantity);
        Assert.Equal(updateProductDto.productCategory, actualResultData.productCategory);
        Assert.Equal(_products[0].ExpiryDateTime.Date, actualResultData.ExpiryDateTime.Date);
    }
    
    [Fact]
    public async Task TestDeleteProductSuccess()
    {
        //Arrange
        var productId = _products[0].productId;
        
        //Mock
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId))
            .ReturnsAsync(_products[0]);
        _productRepository.Setup(repo => repo.DeleteAsync(_products[0].productId))
            .Returns(Task.CompletedTask);
        
        //Act
        var result = await _productController.DeleteProduct(productId);
        
        //Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result).Value;
        var actualResult = Assert.IsType<Result>(okResult);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(200, actualResult.Code);
        Assert.Equal("Delete One Success", actualResult.Message);
        Assert.Null(actualResult.Data);
    }
}