//<copyright file="ProductMapper" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using CachingInDotNet.models;
using CachingInDotNet.models.dto;

namespace CachingInDotNet.mapper;

public class ProductMapper
{
    
    /// <summary>
    /// map from product to productDto
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static ProductDto MapFromProductToProductDto(Product product)
    {
        return new ProductDto(
            product.productId,
            product.productName,
            product.productDescription,
            product.productPrice,
            product.productQuantity,
            product.productCategory,
            product.ExpiryDateTime
        );
    }
    
    
    /// <summary>
    /// map from product list to productDto list
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    public static IEnumerable<ProductDto> MapFromProductListToProductDtoList(IEnumerable<Product> products)
    {
        return products.Select(MapFromProductToProductDto).ToList();
    }
    
    
    /// <summary>
    /// map from createProductDto to product
    /// </summary>
    /// <param name="createProductDto"></param>
    /// <returns></returns>
    public static Product MapFromCreateProductDtoToProduct(CreateProductDto createProductDto)
    {
        return new Product()
        {
            productName = createProductDto.productName,
            productDescription = createProductDto.productDescription,
            productPrice = createProductDto.productPrice,
            productQuantity = createProductDto.productQuantity,
            productCategory = createProductDto.productCategory,
            ExpiryDateTime = createProductDto.ExpiryDateTime,
            productCreatedDate = createProductDto.productCreatedDate
        };
    }
    
    
    /// <summary>
    /// map from updateProductDto to product
    /// </summary>
    /// <param name="updateProductDto"></param>
    /// <returns></returns>
    public static Product MapFromUpdateProductDtoToProduct(UpdateProductDto updateProductDto)
    {
        return new Product()
        {
            productName = updateProductDto.productName,
            productDescription = updateProductDto.productDescription,
            productPrice = updateProductDto.productPrice,
            productQuantity = updateProductDto.productQuantity,
            productCategory = updateProductDto.productCategory,
           
        };
    }
}