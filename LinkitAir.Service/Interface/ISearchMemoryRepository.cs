using System.Collections.Generic;
using System.Threading.Tasks;
using LinkitAir.Service.Dto;
using LinkitAir.Service.Helpers;
using LinkitAir.Service.Model;

namespace LinkitAir.Service.Interface
{
    public interface ISearchMemoryRepository
    {        
        Task<List<Airport>> SearchSourceAirportAsync(AirportSearchDto searchParam);    
        Task<List<Airport>> SearchDestinationAirportAsync(AirportSearchDto searchParam);    
        Task<PagedList<RouteSearchResultDto>> SearchRouteAsync(RouteSearchDto routeParams); 
        Task<PagedList<RouteSearchResultDto>> SearchAllRouteAsync(RouteSearchDto routeParams); 
        Task<RouteSearchResultDto> GetRouteAsync(int routeId);     
    }
}