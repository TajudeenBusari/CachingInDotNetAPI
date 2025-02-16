//<copyright file="ProductController" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>



using CachingInDotNet.mapper;
using CachingInDotNet.models.dto;
using CachingInDotNet.service;
using CachingInDotNet.system;
using Microsoft.AspNetCore.Mvc;

namespace CachingInDotNet.controller;
/// <summary>
/// Product Controller class which handles all product related operations
/// and implements the IProductService interface
/// </summary>
[Route("api/v1/product")]
[ApiController]
public class ProductController: ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Result>> GetAllProducts()
    {
        var products = await _productService.GetProductsAsync();
        //convert to Dto
        var productDtoList = ProductMapper.MapFromProductListToProductDtoList(products);
        return Ok(new Result(true, system.StatusCode.SUCCESS, "Find All Success", productDtoList));
        
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<Result>> GetProductById([FromRoute] Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        //convert to Dto
        var productDto = ProductMapper.MapFromProductToProductDto(product);
        return Ok(new Result(true, system.StatusCode.SUCCESS, "Find One Success", productDto));
    }
    
    /// <summary>
    /// Add product
    /// </summary>
    /// <param name="createRequestProductDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Result>> AddProduct([FromBody] CreateProductDto createRequestProductDto)
    {
         if (!ModelState.IsValid)
         {
             return new Result(false, system.StatusCode.BAD_REQUEST, "Invalid Data", ModelState);
         }
        
         //convert to product domain
         var product = ProductMapper.MapFromCreateProductDtoToProduct(createRequestProductDto);
         var savedProduct = await _productService.AddProductAsync(product);
         //convert back to productDto
         var savedProductDto = ProductMapper.MapFromProductToProductDto(savedProduct);
         return Ok(new Result(true, system.StatusCode.SUCCESS, "Add One Success", savedProductDto));
    }
    
    /// <summary>
    /// Update product by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateProductDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult<Result>> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        
         if (!ModelState.IsValid)
         {
                return new Result(false, system.StatusCode.BAD_REQUEST, "Invalid Data", ModelState);
         }
         //convert to product domain
        var product = ProductMapper.MapFromUpdateProductDtoToProduct(updateProductDto);
        var updatedProduct = await _productService.UpdateProductAsync(id, product);
        //convert back to productDto
        var updatedProductDto = ProductMapper.MapFromProductToProductDto(updatedProduct);
        return Ok(new Result(true, system.StatusCode.SUCCESS, "Update One Success", updatedProductDto));
    }
    
    /// <summary>
    /// Remove product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult<Result>> DeleteProduct([FromRoute] Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return Ok(new Result(true, system.StatusCode.SUCCESS, "Delete One Success"));
    }
    
    /// <summary>
    /// Clear all cache from Redis
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("clear-cache")]
    public async Task<ActionResult<Result>> ClearCache()
    {
        await _productService.ClearAllCacheAsync();
        return Ok(new Result(true, system.StatusCode.SUCCESS, "Clear All Cache Success"));
    }
    
}