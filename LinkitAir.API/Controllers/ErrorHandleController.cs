using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Configurations;
using LinkitAir.Service.Model;
using LinkitAir.Service.Dto;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using LinkitAir.Service.Helpers;
using LinkitAir.Service.ExceptionHandler;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Net;
    
namespace LinkitAir.API.Controllers
{
    /*
    This ErrorController handles some 4xx Client errors caused by users.
    The other errors like 5xx server errors should be handle by GlobalDataCustomException 
     */
    [Route("/errors")]
    [ApiController]
    public class ErrorHandleController : ControllerBase
    {
        private readonly ILogger<SearchElasticController> logger;

        public ErrorHandleController(ILogger<SearchElasticController> logger)
        {
            this.logger = logger;
        }


        [Route("{code}")] //like './errors/404' 4xx Client errors
        public IActionResult Error(int code)
        {
            HttpStatusCode parsedCode = (HttpStatusCode)code;
            ApiError error = new ApiError(code, parsedCode.ToString());
            //This log information will be sent to Elasticsearch by SeriLog which was overridden during startUp
            this.logger.LogError($"linkit-error:{code}");
            return new ObjectResult(error);
        }
    }
}