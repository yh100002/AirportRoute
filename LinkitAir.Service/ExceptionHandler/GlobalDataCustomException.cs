using System.Net;

namespace LinkitAir.Service.ExceptionHandler
{
    public class GlobalDataCustomException : BaseCustomException
    {
        public GlobalDataCustomException(string message, string description) : base(message, description, (int) HttpStatusCode.NotFound)
        {
        }
    }
}