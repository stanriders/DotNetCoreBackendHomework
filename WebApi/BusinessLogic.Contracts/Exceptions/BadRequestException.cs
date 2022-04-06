
namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public class BadRequestException : ErrorCodeException
    {
        public BadRequestException(string errorCode) : base(errorCode)
        {
        }
    }
}