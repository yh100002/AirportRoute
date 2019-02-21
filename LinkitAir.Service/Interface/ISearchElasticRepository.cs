using System.Collections.Generic;
using System.Threading.Tasks;
using LinkitAir.Service.Dto;
using LinkitAir.Service.Helpers;
using LinkitAir.Service.Model;

namespace LinkitAir.Service.Interface
{
    public interface ISearchElasticRepository
    {                  
        Task<List<RouteSearchResultDto>> SearchRouteAsync(RouteSearchDto routeParams);       
        Task<List<Airport>> Autocomplete(string query);
    }
}