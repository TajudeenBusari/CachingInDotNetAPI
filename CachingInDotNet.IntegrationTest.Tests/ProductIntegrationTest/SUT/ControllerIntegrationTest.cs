//<copyright file="ControllerIntegrationTest" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using System.Net;
using System.Net.Http.Json;
using CachingInDotNet.IntegrationTest.Tests.ProductIntegrationTest.Fixtures;
using CachingInDotNet.IntegrationTest.Tests.ProductIntegrationTest.Helper;
using CachingInDotNet.models.dto;
using CachingInDotNet.system;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace CachingInDotNet.IntegrationTest.Tests.ProductIntegrationTest.SUT;

/// <summary>
/// Controller Integration Test
/// </summary>
public class ControllerIntegrationTest: IClassFixture<CustomDockerWebApplicationFactory>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly CustomDockerWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Mock<IConnectionMultiplexer> _mockRedis;
    public ControllerIntegrationTest(CustomDockerWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient();
        
        //Mock the Redis Connection
        _mockRedis = new Mock<IConnectionMultiplexer>();
        _mockRedis
            .Setup(x => 
                x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(Mock.Of<IDatabase>());
    }

    /// <summary>
    /// Test to get all products
    /// </summary>
    /// <returns>Returns a list ProductDto</returns>
    [Fact]
    public async Task TestGetAllProductsSuccess()
    {
        //Arrange
        //Act
        var response = await _client.GetAsync(HttpHelper.Urls.GetAllProducts);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jsonString = await response.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<Result>(jsonString);
        Assert.True(deserializeObject?.IsSuccess);
        Assert.Equal(200, deserializeObject?.Code);
        Assert.Equal("Find All Success", deserializeObject?.Message);
        _testOutputHelper.WriteLine(jsonString);
    }

    /// <summary>
    /// Test to get a product by Id
    /// </summary>
    /// <returns>Returns a ProductDto</returns>
    [Fact]
    public async Task TestGetProductByIdSuccess()
    {
        //Arrange
        var id = new Guid("393e2f50-9361-457f-80e5-c822e14f60fc");
        //Act
        var response = await _client.GetAsync(HttpHelper.Urls.GetProductById(id));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jsonString = await response.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<Result>(jsonString);
        Assert.True(deserializeObject?.IsSuccess);
        Assert.Equal(200, deserializeObject?.Code);
        Assert.Equal("Find One Success", deserializeObject?.Message);
        _testOutputHelper.WriteLine(jsonString);
    }

    /// <summary>
    /// Test to add a product and update the product
    /// </summary>
    /// <returns>Returns a ProductDto</returns>
    [Fact]
    public async Task TestAddProductAndUpdateProductSuccess()
    {
        //Arrange
        var createProductDto = new CreateProductDto(
            "Product 9",
            "Product 1 Description",
            1000,
            10,
                "Category 10",
            DateTime.UtcNow.AddDays(10),//expires in 10 days
            DateTime.UtcNow.AddDays(-2) //created 2 days ago
        );
        
        //Act
        var response = await _client.PostAsJsonAsync(HttpHelper.Urls.AddProduct, createProductDto);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jsonString = await response.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<Result>(jsonString);
        Assert.True(deserializeObject?.IsSuccess);
        Assert.Equal(200, deserializeObject?.Code);
        Assert.Equal("Add One Success", deserializeObject?.Message);
        _testOutputHelper.WriteLine(jsonString);
        
        //Extract the product id and update the product
        var productDto = JsonConvert.DeserializeObject<ProductDto>(deserializeObject?.Data.ToString()!);
        productDto.Should().NotBeNull();
        var productId = productDto.ProductId;
        var updated = new UpdateProductDto(
            "Product 9 Updated",
            "Product 1 Description Updated",
            2000,
            20,
            "Category 10 Updated"
        );
        
        //Act
        var updateResponse = await _client.PutAsJsonAsync(HttpHelper.Urls.UpdateProduct(productId), updated);
        
        //Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updateJsonString = await updateResponse.Content.ReadAsStringAsync();
        var updateDeserializeObject = JsonConvert.DeserializeObject<Result>(updateJsonString);
        Assert.True(updateDeserializeObject?.IsSuccess);
        Assert.Equal(200, updateDeserializeObject?.Code);
        Assert.Equal("Update One Success", updateDeserializeObject?.Message);
        _testOutputHelper.WriteLine(updateJsonString);
        Assert.Equal(productId.ToString(), 
            JsonConvert.DeserializeObject<ProductDto>(updateDeserializeObject?.Data.ToString()!)?.ProductId.ToString());
    }

    /// <summary>
    /// Test to add a product and delete the product
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestAddAndDeleteProductSuccess()
    {
        //Arrange
        var createProductDto = new CreateProductDto(
            "Product 10",
            "Product 10 Description",
            1000,
            10,
            "Category 10",
            DateTime.UtcNow.AddDays(10),//expires in 10 days
            DateTime.UtcNow.AddDays(-2) //created 2 days ago
        );
        
        //Act
        var response = await _client.PostAsJsonAsync(HttpHelper.Urls.AddProduct, createProductDto);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jsonString = await response.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<Result>(jsonString);
        Assert.True(deserializeObject?.IsSuccess);
        Assert.Equal(200, deserializeObject?.Code);
        Assert.Equal("Add One Success", deserializeObject?.Message);
        var productDto = JsonConvert.DeserializeObject<ProductDto>(deserializeObject?.Data.ToString()!);
        var productId = productDto!.ProductId;
        
        //Delete the product
        var deleteResponse = await _client.DeleteAsync(HttpHelper.Urls.DeleteProduct(productId));
        var deleteJsonString = await deleteResponse.Content.ReadAsStringAsync();
        var deleteDeserializeObject = JsonConvert.DeserializeObject<Result>(deleteJsonString);
        Assert.True(deleteDeserializeObject?.IsSuccess);
        Assert.Equal(200, deleteDeserializeObject?.Code);
        Assert.Equal("Delete One Success", deleteDeserializeObject?.Message);
        
    }

    /// <summary>
    /// Test to clear the cache
    /// </summary>
    /// <remarks> Since clear cache is a Post request which does 
    /// not require any data, we can pass an empty object
    /// </remarks>
    /// <returns></returns>
    [Fact]
    public async Task TestClearCacheSuccess()
    {
        //Arrange
        //Act
        var response = await _client.PostAsJsonAsync(HttpHelper.Urls.ClearCache, new {});
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jsonString = await response.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<Result>(jsonString);
        Assert.True(deserializeObject?.IsSuccess);
        Assert.Equal(200, deserializeObject?.Code);
        Assert.Equal("Clear All Cache Success", deserializeObject?.Message);
        _testOutputHelper.WriteLine(jsonString);
        
    }
    
}