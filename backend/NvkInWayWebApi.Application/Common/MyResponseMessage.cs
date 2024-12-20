using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common
{
    public class MyResponseMessage
    {
        public string Message { get; set; }

        public List<ValidationErrorResponse> Errors { get; set; }

        public MyResponseMessage(string message) : this(message, new List<ValidationErrorResponse>())
        {
        }

        public MyResponseMessage(string message, List<ValidationErrorResponse> errors)
        {
            Message = message;
            Errors = errors;
        }
    }

    public class ValidationErrorResponse
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationErrorResponse()
        {

        }

        public ValidationErrorResponse(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
