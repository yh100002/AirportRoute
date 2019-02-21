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
using Microsoft.Extensions.Logging;
using Nest;

namespace LinkitAir.Service.Data
{
    public class SearchElasticRepository : ISearchElasticRepository
    {
        private readonly IElasticClient elasticClient;
        private readonly ILogger<SearchElasticRepository> logger;
        private readonly IMapper mapper;

        public SearchElasticRepository(ElasticClientProvider provider, ILogger<SearchElasticRepository> logger, IMapper mapper)
        {
            this.elasticClient = provider.Client;
            this.logger = logger;
            this.mapper = mapper;
        }        

        public async Task<List<RouteSearchResultDto>> SearchRouteAsync(RouteSearchDto routeParams)
        {
            var foundRouteIndexes = await this.elasticClient.SearchAsync<RouteIndex>(searchDescriptor => searchDescriptor
                    .Query(queryContainerDescriptor => queryContainerDescriptor
                        .Bool(queryDescriptor => queryDescriptor
                            .Must(queryStringQuery => queryStringQuery
                                .QueryString(queryString => queryString
                                    .Query(routeParams.FullTextSearch))))) 
                                        .From((routeParams.PageNumber - 1) * routeParams.PageSize)
                                        .Size(routeParams.PageSize));   

            var resultSet = this.mapper.Map<List<RouteSearchResultDto>>(foundRouteIndexes.Documents);     
            
            return PagedList<RouteSearchResultDto>.Create(resultSet.AsQueryable(), routeParams.PageNumber, routeParams.PageSize);  
        }
        
        /*
        The completion suggester also supports fuzzy queries.this means, you can have a typo in your search and still get results back.
        But it should be hard to have our customer undestood internal algorithm but very useful
         */
        public async Task<List<Airport>> Autocomplete(string airportName) 
        {
            var response = await this.elasticClient.SearchAsync<RouteIndex>(sr => sr
                .Suggest(scd => scd
                    .Completion("airportname-completion", cs => cs
                        .Prefix(airportName)
                        .Fuzzy(fsd => fsd 
                        .Fuzziness(Fuzziness.Auto)) //using auto weight value, of course we can define our own weight value.
                        .Field(r => r.DestinationAirportName))));

            List<Airport> suggestions = this.ExtractAutocompleteSuggestions(response);            
            return suggestions;
        }

        private List<Airport> ExtractAutocompleteSuggestions(ISearchResponse<RouteIndex> response) 
        { 
            List<Airport> results = new List<Airport>();
            var suggestions = response.Suggest["airportname-completion"].Select(s => s.Options);
            foreach (var suggestionsCollection in suggestions)
            {
                foreach (var suggestion in suggestionsCollection)
                {
                    var suggestedRoute = suggestion.Source;
                    var autocompleteResult = new Airport
                    {
                        AirportId = suggestedRoute.DestinationAirportId,
                        AirportName = suggestedRoute.DestinationAirportName
                    };

                    results.Add(autocompleteResult);
                }
            }

            return results;
        }
    }
}