//<copyright file="CreateProductDto" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CachingInDotNet.models.dto;

public record CreateProductDto(
    [Required]
    [MaxLength(25, ErrorMessage = "Product name cannot be more than 25 characters.")]
    [MinLength(3, ErrorMessage = "Product name cannot be less than 3 characters.")]
    string productName,
    
    [Required]
    [MaxLength(100, ErrorMessage = "Product description cannot be more than 100 characters.")]
    [MinLength(3, ErrorMessage = "Product description cannot be less than 3 characters.")]
    string productDescription,
    
    [Required]
    [Range(0.01, 1000000, ErrorMessage = "Product price must be between 0.01 and 1000000.")]
    double productPrice,
    
    [Required]
    [Range(1, 1000000, ErrorMessage = "Product quantity must be between 1 and 1000000.")]
    int productQuantity,
    
    [Required]
    [MaxLength(30, ErrorMessage = "Product category cannot be more than 25 characters.")]
    [MinLength(3, ErrorMessage = "Product category cannot be less than 3 characters.")]
    string productCategory,
    
    [Required]
    DateTime ExpiryDateTime,
    [Required] 
    DateTime productCreatedDate
    )
{
    [JsonPropertyName("productCreatedDate")]
    //public DateTime productCreatedDate { get; init; } = productCreatedDate == default ? DateTime.UtcNow : productCreatedDate.ToUniversalTime();
    public DateTime productCreatedDate { get; init; } = ValidateProductCreatedDate(productCreatedDate);
    
    [JsonPropertyName("expiryDateTime")]
    //public DateTime ExpiryDateTime { get; init; } = ExpiryDateTime == default ? DateTime.UtcNow.AddYears(1) : ExpiryDateTime.ToUniversalTime();
    public DateTime ExpiryDateTime { get; init; } = ValidateExpiryDateTime(ExpiryDateTime);
    
    /// <summary>
    /// Validate the product created date
    /// </summary>
    /// <param name="productCreatedDate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static DateTime ValidateProductCreatedDate(DateTime productCreatedDate)
    {
        var utcDate = DateTime.SpecifyKind(productCreatedDate, DateTimeKind.Utc);
        if (utcDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Product created date cannot be in the future.", nameof(productCreatedDate));
        }
        return utcDate;
    }
    
    /// <summary>
    /// Validate the expiry date
    /// </summary>
    /// <param name="expiryDateTime"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static DateTime ValidateExpiryDateTime(DateTime expiryDateTime)
    {
        var utcDate = DateTime.SpecifyKind(expiryDateTime, DateTimeKind.Utc);
        if (utcDate < DateTime.UtcNow)
        {
            throw new ArgumentException("Expiry date cannot be in the past.", nameof(expiryDateTime));
        }
        return utcDate;
    }
    
}