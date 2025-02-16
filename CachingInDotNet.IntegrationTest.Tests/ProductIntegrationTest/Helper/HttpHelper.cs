//<copyright file="HttpHelper" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
namespace CachingInDotNet.IntegrationTest.Tests.ProductIntegrationTest.Helper;

/// <summary>
/// This class contains all the Urls for the API endpoints
/// </summary>
public class HttpHelper
{
    internal static class Urls
    {
        public static string GetAllProducts = "api/v1/product";
        public static string GetProductById(Guid id) => $"api/v1/product/{id}";
        public static string AddProduct = "api/v1/product";
        public static string UpdateProduct(Guid id) => $"api/v1/product/{id}";
        public static string DeleteProduct(Guid id) => $"api/v1/product/{id}";
         public static string ClearCache = "api/v1/product/clear-cache";
    }
    
}