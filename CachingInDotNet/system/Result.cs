namespace CachingInDotNet.system;

public class Result
{
    public bool IsSuccess { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    
    public Result()
    {
        
    }
    public Result(bool isSuccess, int code, string message, object data)
    {
        IsSuccess = isSuccess;
        Code = code;
        Message = message;
        Data = data;
    }
    
    public Result(bool isSuccess, int code, string message)
    {
        IsSuccess = isSuccess;
        Code = code;
        Message = message;
        
    }
}