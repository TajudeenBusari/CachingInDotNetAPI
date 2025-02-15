//<copyright file="ProductDto" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
namespace CachingInDotNet.models.dto;

public record ProductDto(
    Guid ProductId,
    string productName,
    string productDescription,
    double productPrice,
    int productQuantity,
    string productCategory,
    DateTime ExpiryDateTime)
{
    
};