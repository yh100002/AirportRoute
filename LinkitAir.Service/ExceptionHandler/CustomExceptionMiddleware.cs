using System;
using System.Net;
using System.Threading.Tasks;
using LinkitAir.Service.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LinkitAir.Service.ExceptionHandler
{
    /*
    I recommend that you create this middleware as seperate assembly module.
    Small is better.
     */
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<CustomExceptionMiddleware> logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /*
        All unhandled exceptions can reach this place and so you can handle them by puuting own special log information whatever you want.
         */
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as BaseCustomException;
            var statusCode = (int) HttpStatusCode.InternalServerError;// I assume all is 500
            var message = $"linkit-error:{statusCode}";
            var description = $"Unexpected error :{exception.Message}";

            if (null != customException)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
            }            

            this.logger.LogError($"Linkit Unhandled Exception:{message}");            

            var error = context.Features.Get<IExceptionHandlerFeature>();
            if(error != null) response.AddApplicationError(error.Error.Message);      
            
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
                  
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = message,
                Description = description
            }));
            }
        }
}