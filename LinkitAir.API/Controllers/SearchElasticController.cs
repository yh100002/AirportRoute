
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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;


namespace LinkitAir.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchElasticController : ControllerBase
    {
        private readonly ISearchElasticRepository searchElasticRepository;       
        private readonly ILogger<SearchElasticController> logger;

        public SearchElasticController(ISearchElasticRepository searchElasticRepository, ILogger<SearchElasticController> logger)
        {
            this.searchElasticRepository = searchElasticRepository;
            this.logger = logger;        
        }  

        [HttpGet("searchroute")]
        public async Task<IActionResult> SearchRoute([FromQuery] RouteSearchDto routeParams)
        {              
           var routeSearchResults = await this.searchElasticRepository.SearchRouteAsync(routeParams);     
           return Ok(routeSearchResults);
        } 

        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete([FromQuery] AirportSearchDto query)
        {
            var fuzzyResult = await this.searchElasticRepository.Autocomplete(query.AirportName);
            return Ok(fuzzyResult);
        }
    }
}