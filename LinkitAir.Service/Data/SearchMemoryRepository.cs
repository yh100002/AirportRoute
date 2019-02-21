using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinkitAir.Service.Dto;
using LinkitAir.Service.Helpers;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace LinkitAir.Service.Data
{    
    public class SearchMemoryRepository : ISearchMemoryRepository
    {
        //I should be better to make 'unit of work' pattern which contains all repogitories.
        private readonly DataContext context;
         private readonly IMapper mapper;
        public SearchMemoryRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<RouteSearchResultDto> GetRouteAsync(int routeId)
        {              
            var results = await (from r in this.context.Routes where r.RouteId == routeId
            join a1 in this.context.Airports on r.SourceAirportId equals a1.AirportId
            join a2 in this.context.Airports on r.DestinationAirportId equals a2.AirportId
            join al in this.context.Airlines on r.AirlineID equals al.AirlineId
            select new RouteIndex
            {
                RouteId = r.RouteId,
                FlightFare = r.FlightFare,
                DepartureDateTime = r.DepartureDateTime,
                ArrivalDateTime = r.ArrivalDateTime,
                SourceAirportName = a1.AirportName,
                SourceAirportId = a1.AirportId,
                SourceAirportCity = a1.City,
                SourceAirportCountry = a1.Country,
                DestinationAirportName = a2.AirportName,
                DestinationAirportId = a2.AirportId,
                DestinationAirportCity = a2.City,
                DestinationAirportCountry = a2.Country,
                AirlineName = al.AirlineName,
                SourceLat = a1.Lat,
                SourceLon = a1.Lon,
                DestinationLat = a2.Lat,
                DestinationLon = a2.Lon                      
            }).FirstOrDefaultAsync();

            //In real world scenario, mapping must be a lot more complicated but for this test project there is almost nothing to do. 
            var resultDto = this.mapper.Map<RouteSearchResultDto>(results);
               
            return resultDto;
        }

        public async Task<PagedList<RouteSearchResultDto>> SearchRouteAsync(RouteSearchDto routeParams)
        {             
            var results = await (from r in this.context.Routes 
                            where r.SourceAirportId == routeParams.SourceAirportId && r.DestinationAirportId == routeParams.DestinationAirportId
                            join a1 in this.context.Airports on r.SourceAirportId equals a1.AirportId    
                            join a2 in this.context.Airports on r.DestinationAirportId equals a2.AirportId
                            join al in this.context.Airlines on r.AirlineID equals al.AirlineId
                            select new RouteIndex
                                        {
                                            RouteId = r.RouteId,
                                            FlightFare = r.FlightFare,
                                            DepartureDateTime = r.DepartureDateTime,
                                            ArrivalDateTime = r.ArrivalDateTime,
                                            SourceAirportName = a1.AirportName,
                                            SourceAirportId = a1.AirportId,
                                            SourceAirportCity = a1.City,
                                            SourceAirportCountry = a1.Country,
                                            DestinationAirportName = a2.AirportName,
                                            DestinationAirportId = a2.AirportId,
                                            DestinationAirportCity = a2.City,
                                            DestinationAirportCountry = a2.Country,
                                            AirlineName = al.AirlineName,
                                            SourceLat = a1.Lat,
                                            SourceLon = a1.Lon,
                                            DestinationLat = a2.Lat,
                                            DestinationLon = a2.Lon                             
                                        }).ToListAsync();


            var routeResults = this.mapper.Map<List<RouteSearchResultDto>>(results);

            var resultSet = PagedList<RouteSearchResultDto>.Create(routeResults.AsQueryable(), routeParams.PageNumber, routeParams.PageSize);       
            return  resultSet; 
        }

        public async Task<List<Airport>> SearchSourceAirportAsync(AirportSearchDto searchParam)
        {
            var foundAirports = await this.context.Airports.Where(s => s.AirportName.Contains(searchParam.AirportName)
                 || s.Country.Contains(searchParam.AirportName)).ToListAsync();  

            return foundAirports;
        }

        public async Task<List<Airport>> SearchDestinationAirportAsync(AirportSearchDto searchParam)
        {
            var foundAirports = await this.context.Airports.Where(s => s.AirportName.Contains(searchParam.AirportName)
                 || s.Country.Contains(searchParam.AirportName)).ToListAsync();  
            return foundAirports;
        }
        
        public async Task<PagedList<RouteSearchResultDto>> SearchAllRouteAsync(RouteSearchDto routeParams)
        { 
            /*
            Actually I don't have to create new instance of RouteIndex in this test project.
            But in real scenario I recommend that you split with model and dto using some mapper to instantiate from model to dto.
             */
            var results = await (from r in this.context.Routes
                        join a1 in this.context.Airports on r.SourceAirportId equals a1.AirportId
                        join a2 in this.context.Airports on r.DestinationAirportId equals a2.AirportId
                        join al in this.context.Airlines on r.AirlineID equals al.AirlineId
                        select new RouteIndex
                        {
                            RouteId = r.RouteId,
                            FlightFare = r.FlightFare,
                            DepartureDateTime = r.DepartureDateTime,
                            ArrivalDateTime = r.ArrivalDateTime,
                            SourceAirportName = a1.AirportName,
                            SourceAirportId = a1.AirportId,
                            SourceAirportCity = a1.City,
                            SourceAirportCountry = a1.Country,
                            DestinationAirportName = a2.AirportName,
                            DestinationAirportId = a2.AirportId,
                            DestinationAirportCity = a2.City,
                            DestinationAirportCountry = a2.Country,
                            AirlineName = al.AirlineName,
                            SourceLat = a1.Lat,
                            SourceLon = a1.Lon,
                            DestinationLat = a2.Lat,
                            DestinationLon = a2.Lon                             
                        }).ToListAsync();

            var routeResults = this.mapper.Map<List<RouteSearchResultDto>>(results);

            return PagedList<RouteSearchResultDto>.Create(routeResults.AsQueryable(), routeParams.PageNumber, routeParams.PageSize);     
        }
    }
}
