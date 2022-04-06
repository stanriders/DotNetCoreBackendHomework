namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public class NotFoundException : ErrorCodeException
    {
        public NotFoundException(string message) : base("NotFound", message)
        {
        }
    }
}
