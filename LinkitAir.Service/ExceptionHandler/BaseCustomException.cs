using System;

namespace LinkitAir.Service.ExceptionHandler
{
    public class BaseCustomException : Exception
    {
        public int Code { get; set; }

        public string Description{ get; set; }        

        public BaseCustomException(string message, string description, int code) : base(message)
        {
            this.Code = code;
            this.Description = description;
        }
    }
}