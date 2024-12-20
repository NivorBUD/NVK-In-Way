using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorText { get; set; }
        public int StatusCode { get; set; }

        public static OperationResult Success(int statusCode = 200)
        {
            return new OperationResult
            {
                IsSuccess = true,
                ErrorText = null,
                StatusCode = statusCode
            };
        }

        public static OperationResult Error(string errorText, int statusCode = 400)
        {
            return new OperationResult()
            {
                IsSuccess = false,
                ErrorText = errorText,
                StatusCode = statusCode
            };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public static OperationResult<T> Success(T data = default, int statusCode = 200)
        {
            return new OperationResult<T>
            {
                IsSuccess = true,
                ErrorText = null,
                StatusCode = statusCode,
                Data = data
            };
        }

        public new static OperationResult<T> Error(string errorText, int statusCode = 400)
        {
            return new OperationResult<T>()
            {
                IsSuccess = false,
                ErrorText = errorText,
                StatusCode = statusCode
            };
        }
    }
}
