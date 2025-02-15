//<copyright file="ExceptionHandlingMiddleWare" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>

using CachingInDotNet.system;
using ValidationException = FluentValidation.ValidationException;

namespace CachingInDotNet.exception;

public class ExceptionHandlingMiddleWare
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleWare(RequestDelegate next)
    {
        _next = next;
        
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

        }
        catch (ProductNotFoundException ex)
        {
            await HandleProductNotFoundException(context, ex, 404);

        }
        catch (ValidationException ex)
        {
            await HandleInvalidDataException(context, ex, 400);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, 500, ex);
        }
        
    }

    /// <summary>
    /// Handle the ProductNotFoundException
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <param name="code"></param>
    private async Task HandleProductNotFoundException(HttpContext context, Exception exception, int code)
    {
        var result = new Result()
        {
            IsSuccess = false,
            Code = code,
            Message = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        await context.Response.WriteAsJsonAsync(result);
    }
    
    /// <summary>
    /// The validationException is from the FluentValidation library
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="ex"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    private Task HandleInvalidDataException(HttpContext httpContext, ValidationException ex, int code)
    {
       var errors = ex.Errors.ToDictionary
           (
           e => e.PropertyName, 
           e => new []{e.ErrorMessage}
           );
         var result = new Result()
         {
             IsSuccess = false,
             Code = code,
             Message = "Invalid Data",
             Data = errors
         };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = code;
            return httpContext.Response.WriteAsJsonAsync(result);
    }
    
    /// <summary>
    /// This is a generic exception handler, it is thrown when an unexpected error occurs
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="code"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    private Task HandleGenericExceptionAsync(HttpContext httpContext, int code, Exception ex)
    {
        var result = new Result()
        {
            IsSuccess = false,
            Code = StatusCode.INTERNAL_SERVER_ERROR,
            Message = "An unexpected error occurred"
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = code;
        return httpContext.Response.WriteAsJsonAsync(result);
    }
}