
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
    public class SearchMemoryController : ControllerBase
    {
        private readonly ISearchMemoryRepository searchMemoryRepository;       
        private readonly ILogger<SearchMemoryController> logger;

        public SearchMemoryController(ISearchMemoryRepository searchMemoryRepository, ILogger<SearchMemoryController> logger)
        {
            this.searchMemoryRepository = searchMemoryRepository;
            this.logger = logger;           
        }

        [HttpGet("searchsourceairport")]
        public async Task<IActionResult> SearchSourceAirport([FromQuery] AirportSearchDto airportParam)
        {
            var sourceSearchResults = await this.searchMemoryRepository.SearchSourceAirportAsync(airportParam);
            return Ok(sourceSearchResults);
        }

        [HttpGet("searchdestinationairport")]
        public async Task<IActionResult> SearchDestinationAirport([FromQuery] AirportSearchDto airportParam)
        {
             var destinationSearchResults = await this.searchMemoryRepository.SearchDestinationAirportAsync(airportParam);
            return Ok(destinationSearchResults);
        }

        [HttpGet("searchroute")]
        public async Task<IActionResult> SearchRoute([FromQuery] RouteSearchDto routeParams)
        {              
           var routeSearchResults = await this.searchMemoryRepository.SearchRouteAsync(routeParams);           
           Response.AddPagination(routeSearchResults.CurrentPage, routeSearchResults.PageSize, routeSearchResults.TotalCount, routeSearchResults.TotalPages);           
           return Ok(routeSearchResults);
        }

        [HttpGet("searchallroute")]
        public async Task<IActionResult> SearchAllRoute([FromQuery] RouteSearchDto routeParams)
        {              
           var routeSearchResults = await this.searchMemoryRepository.SearchAllRouteAsync(routeParams);           
           Response.AddPagination(routeSearchResults.CurrentPage, routeSearchResults.PageSize, routeSearchResults.TotalCount, routeSearchResults.TotalPages);           
           return Ok(routeSearchResults);
        }

        [HttpGet("{routeId}")]
        public async Task<IActionResult> Get(int routeId)
        {
           var routeGetResult = await this.searchMemoryRepository.GetRouteAsync(routeId);          
           return Ok(routeGetResult);
        }

    }
}