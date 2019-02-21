using System.Linq;
using AutoMapper;
using LinkitAir.Service.Dto;
using LinkitAir.Service.Model;

namespace LinkitAir.Service.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {           
            //In real world scenario, mapping must be a lot more complicated but for this test project there is almost nothing to do. 
           CreateMap<PassengerForRegisterDto, Passenger>();
           CreateMap<RouteIndex, RouteSearchResultDto>();
           CreateMap<Route, RouteSearchResultDto>();
        }        
    }
}