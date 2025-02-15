namespace CachingInDotNet.exception;

public class ProductNotFoundException: Exception
{
    public ProductNotFoundException(Guid id) : base("Couldn't find product with id " + id) { }
    
}