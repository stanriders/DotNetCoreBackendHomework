using System;

namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public class ErrorCodeException : Exception
    {
        public string ErrorCode { get; }

        public ErrorCodeException(string errorCode, string? userMessage = null) : base(userMessage)
        {
            ErrorCode = errorCode;
        }
    }
}
