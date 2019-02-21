using Microsoft.IdentityModel.Tokens;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Configurations;
using LinkitAir.Service.Model;
using LinkitAir.Service.Dto;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using LinkitAir.Service.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using LinkitAir.Service.ExceptionHandler;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Elasticsearch.Net;
using System;
using Microsoft.Extensions.Options;
using LinkitAir.Service.Configuration;


namespace LinkitAir.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogViewController : ControllerBase
    {
        private readonly ILogViewRepository logViewRepository;
        public LogViewController(ILogViewRepository logViewRepository)     
        {
            this.logViewRepository = logViewRepository;        
        }

        [HttpGet("Overall")]
        public IActionResult Overall()
        {
            var aggregatedValues = this.logViewRepository.AggregatedValuesForAll();
            var logAggregatedValueDto = new LogAggregatedValueDto()
            {
                TotalCountOfAllRequest = this.logViewRepository.Total().Value,
                TotalCountOfNormal = this.logViewRepository.CountOnLevel("information"),                      
                TotalCountError = this.logViewRepository.CountOnLevel("error"),
                AverageDurationForAllRequest = aggregatedValues.AverageDurationForAllRequest,
                MaxDurationForAllRequest = aggregatedValues.MaxDurationForAllRequest,
                MinDurationForAllRequest = aggregatedValues.MinDurationForAllRequest,
                CountOfEachErrors = this.logViewRepository.GetErrorSet()   
            };

            return Ok(logAggregatedValueDto);                                      
        } 
    }

    
}