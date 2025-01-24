public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string ErrorText { get; set; }
    public int StatusCode { get; set; }

    public static ApiResponse Success(int statusCode = 200)
    {
        return new ApiResponse
        {
            IsSuccess = true,
            ErrorText = null,
            StatusCode = statusCode
        };
    }

    public static ApiResponse Error(string errorText, int statusCode = 400)
    {
        return new ApiResponse
        {
            IsSuccess = false,
            ErrorText = errorText,
            StatusCode = statusCode
        };
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }

    public static ApiResponse<T> Success(T data = default, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            ErrorText = null,
            StatusCode = statusCode,
            Data = data
        };
    }

    public new static ApiResponse<T> Error(string errorText, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            ErrorText = errorText,
            StatusCode = statusCode,
            Data = default // Установите значение по умолчанию для Data
        };
    }
}