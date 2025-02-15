//<copyright file="UpdateProductDto" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
using System.ComponentModel.DataAnnotations;

namespace CachingInDotNet.models.dto;

public record UpdateProductDto(
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
    string productCategory
    )
{
    
};