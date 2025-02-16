
//<copyright file="ProductController" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
namespace CachingInDotNet.exception;


public class ProductNotFoundException: Exception
{
    /// <summary>
    /// Contains the ProductNotFoundException method
    /// which is used to represent an exception that is thrown
    ///  when a product is not found in the database
    /// </summary>
    /// <param name="id"></param>
    public ProductNotFoundException(Guid id) : base("Couldn't find product with id " + id) { }
    
}